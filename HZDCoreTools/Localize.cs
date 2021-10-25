using CommandLine;
using Decima;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace HZDCoreTools
{
    public static class Localize
    {
        private static readonly string[] _validExtensions = new string[]
        {
            ".bin",
            ".core",
        };

        public class LocalizationCommand
        {
            [Option('i', "input", Required = true, HelpText = "OS input path for game data (.bin, .core). Wildcards (*) supported.")]
            public string InputPath { get; set; }

            [Option('l', "language", Required = true, HelpText = "Language to use")]
            public Decima.HZD.ELanguage Language { get; set; }

            [Option('d', "delimiter", HelpText = "Line delimiter/separator", Default = '|')]
            public char Delimiter { get; set; }
        }

        [Verb("exporttext", HelpText = "Export game localization to a text file")]
        public class ExportLocalizationCommand : LocalizationCommand
        {
            [Option('o', "output", Required = true, HelpText = "OS output path for the generated translation file")]
            public string OutputPath { get; set; }
        }

        [Verb("importtext", HelpText = "Import game localization from a text file and optionally create an archive")]
        public class ImportLocalizationCommand : LocalizationCommand
        {
            [Option('o', "output", Required = true, HelpText = "OS output directory for modified core files. Specify a .bin file to generate an archive instead.")]
            public string OutputPath { get; set; }

            [Option('t', "translationfile", Required = true, HelpText = "OS input path for file containing translated text")]
            public string InputTranslationFile { get; set; }

            [Option('s', "skipunchanged", HelpText = "Skip replacement of unchanged lines", Default = true)]
            public bool SkipUnchanged { get; set; }
        }

        static IEnumerable<(string corePath, Stream Stream)> CreateFileStreamEnumerator(IEnumerable<(string Absolute, string Relative)> files, bool useRawCoreFiles, Predicate<string> filter = null)
        {
            // If no filter is supplied, default to passing everything
            filter ??= (x => true);

            if (useRawCoreFiles)
            {
                // Core files stored on disk
                foreach (var file in files)
                {
                    var corePath = Packfile.SanitizePath(file.Relative);

                    if (!filter(corePath))
                        continue;

                    yield return (corePath, File.OpenRead(file.Absolute));
                }
            }
            else
            {
                // Core files stored in archives
                using var device = new PackfileDevice();

                foreach (var (archive, relative) in files)
                {
                    Console.Write($"Mounting {relative}... ");

                    if (device.Mount(archive))
                        Console.WriteLine("Succeeded");
                    else
                        Console.WriteLine("Failed");
                }

                foreach (ulong pathId in device.ActiveFiles)
                {
                    if (!device.PathIdToFileName(pathId, out string corePath))
                        throw new Exception("Couldn't resolve a pathId to a file name. File enumeration isn't possible.");

                    if (!filter(corePath))
                        continue;

                    // TODO: ExtractFile is running on a single thread here. How do I parallelize? Lazy<>?
                    var stream = new MemoryStream();
                    device.ExtractFile(pathId, stream);

                    yield return (corePath, stream);
                }
            }
        }

        public static void ExportLocalization(ExportLocalizationCommand options)
        {
            var sourceFiles = Util.GatherFiles(options.InputPath, _validExtensions, out string ext);
            var coresToExtract = CreateFileStreamEnumerator(sourceFiles, ext == ".core");

            // For each core file...
            var allTextLines = new ConcurrentBag<string>();

            coresToExtract.AsParallel().ForAll(file =>
            {
                var coreBinary = CoreBinary.FromData(new BinaryReader(file.Stream));

                // Dump all instances of LocalizedTextResource
                foreach (var textResource in coreBinary.Objects.OfType<Decima.HZD.LocalizedTextResource>())
                {
                    var delim = options.Delimiter;
                    var data = textResource.GetStringForLanguage(options.Language);

                    data = data.Replace("\r\n", "<NEWLINE_CRLF>");
                    data = data.Replace("\r", "<NEWLINE_CR>");
                    data = data.Replace("\n", "<NEWLINE_LF>");

                    allTextLines.Add($"{delim}{file.corePath}{delim}{textResource.ObjectUUID}{delim}{data}{delim}");
                }
            });

            File.WriteAllLines(options.OutputPath, allTextLines.OrderBy(x => x));

            Console.WriteLine($"Total lines extracted: {allTextLines.Count}");
        }

        public static void ImportLocalization(ImportLocalizationCommand options)
        {
            var allLines = File.ReadAllLines(options.InputTranslationFile);
            var translationData = new Dictionary<string, List<(string ObjectUUID, string TextData)>>();

            // Tokenize each line according to ExportLocalization
            for (int i = 0; i < allLines.Length; i++)
            {
                var tokens = allLines[i].Split(options.Delimiter);

                if (tokens.Length != 5)
                    throw new FormatException($"Tokenization failed on line {i}. Invalid data format.");

                var corePath = tokens[1];
                var objectUUID = tokens[2];
                var textData = tokens[3];

                textData = textData.Replace("<NEWLINE_CRLF>", "\r\n");
                textData = textData.Replace("<NEWLINE_CR>", "\r");
                textData = textData.Replace("<NEWLINE_LF>", "\n");

                if (!translationData.ContainsKey(corePath))
                    translationData.Add(corePath, new List<(string, string)>());

                translationData[corePath].Add((objectUUID, textData));
            }

            // Open each core file in parallel and insert the data
            var sourceFiles = Util.GatherFiles(options.InputPath, _validExtensions, out string ext);
            var coresToModify = CreateFileStreamEnumerator(sourceFiles, ext == ".core", x => translationData.ContainsKey(x));

            int linesUpdated = 0;
            var patchedCores = new ConcurrentDictionary<string, CoreBinary>();

            coresToModify.AsParallel().ForAll(file =>
            {
                var coreBinary = CoreBinary.FromData(new BinaryReader(file.Stream));
                bool updated = false;

                foreach (var translation in translationData[file.corePath])
                {
                    var textResource = coreBinary.Objects
                        .OfType<Decima.HZD.LocalizedTextResource>()
                        .Single(x => x.ObjectUUID == translation.ObjectUUID);

                    // Don't patch unmodified lines
                    if (options.SkipUnchanged)
                    {
                        if (textResource.GetStringForLanguage(options.Language).Equals(translation.TextData))
                            continue;
                    }

                    textResource.SetStringForLanguage(options.Language, translation.TextData);

                    Interlocked.Increment(ref linesUpdated);
                    updated = true;
                }

                // Keep track of modified objects
                if (updated)
                {
                    patchedCores.TryAdd(file.corePath, coreBinary);
                    Console.WriteLine($"Updated {file.corePath}");
                }
            });

            if (Path.HasExtension(options.OutputPath))
            {
                Console.WriteLine("Creating archive...");

                // Build archive
                //var packfile = new PackfileWriterFast(options.OutputPath, false, FileMode.Create);
                throw new NotImplementedException();
            }
            else
            {
                Console.WriteLine("Writing cores to disk...");
                Directory.CreateDirectory(options.OutputPath);

                // Write all core file data back to their individual files on disk
                foreach (var (corePath, core) in patchedCores)
                {
                    string diskPath = Path.Combine(options.OutputPath, corePath);

                    Directory.CreateDirectory(Path.GetDirectoryName(diskPath));
                    core.ToFile(diskPath);
                }
            }

            Console.WriteLine($"Total lines updated: {linesUpdated}");
            Console.WriteLine($"Total cores patched: {patchedCores.Count}");
        }
    }
}
