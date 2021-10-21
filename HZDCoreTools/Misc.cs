using CommandLine;
using Decima;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace HZDCoreTools
{
    public static class Misc
    {
        [Verb("exportstrings", HelpText = "Extract all strings contained in a set of archives")]
        public class ExportAllStringsCommand
        {
            [Option('i', "input", Required = true, HelpText = "Physical input path for game data (.bin). Wildcards (*) supported.")]
            public string InputPath { get; set; }

            [Option('o', "output", Required = true, HelpText = "Physical output path for the generated text file")]
            public string OutputPath { get; set; }

            [Option("validpathsonly", HelpText = "Only dump strings that contain valid core file paths")]
            public bool ValidPathsOnly { get; set; }
        }

        [Verb("exportindexstrings", HelpText = "Extract all paths contained in a set of archive index files")]
        public class ExportIndexFilesCommand
        {
            [Option('i', "input", Required = true, HelpText = "Physical input path for game data (.idx). Wildcards (*) supported.")]
            public string InputPath { get; set; }

            [Option('o', "output", Required = true, HelpText = "Physical output path for the generated text file")]
            public string OutputPath { get; set; }
        }

        public static void ExportAllStrings(ExportAllStringsCommand options)
        {
            var sourceBins = Util.GatherFiles(options.InputPath, new[] { ".bin" }, out _);

            // Mount archives
            using var device = new PackfileDevice();

            foreach (var (archive, relative) in sourceBins)
            {
                Console.WriteLine($"Mounting {relative}...");
                device.Mount(archive);
            }

            // For each core file...
            var allStrings = new ConcurrentDictionary<string, bool>();

            device.ActiveFiles.AsParallel().ForAll(fileId =>
            {
                var stream = new MemoryStream();
                device.ExtractFile(fileId, stream);

                // There's no way to determine if a file is a .core or a .stream.core. This might throw an exception.
                try
                {
                    stream.Position = 0;
                    var coreBinary = CoreBinary.FromData(new BinaryReader(stream), true);

                    coreBinary.VisitAllObjects((string str, object _) =>
                    {
                        allStrings.TryAdd(str, true);
                    });
                }
                catch (InvalidDataException)
                {
                }
                catch (EndOfStreamException)
                {
                }
            });

            if (options.ValidPathsOnly)
            {
                // Now test all possilbe core paths from the given strings
                var possibleLanguages = Enum.GetNames(typeof(Decima.HZD.ELanguage)).Select(x => x.ToLower());
                var allValidPaths = new ConcurrentDictionary<string, bool>();

                foreach (string key in allStrings.Keys)
                {
                    string pathStr = Util.RemoveMountPrefixes(key);
                    pathStr = pathStr.Replace(Packfile.StreamExt, "");
                    pathStr = pathStr.Replace(Packfile.CoreExt, "");

                    //
                    // Possible extensions to check for:
                    // path/to/file.core
                    // path/to/file.coredebug
                    // path/to/file.coretext
                    // path/to/file.dep
                    // path/to/file.core.stream
                    // path/to/file.language.stream
                    //
                    void testPath(string p)
                    {
                        if (device.HasFile(p))
                            allValidPaths.TryAdd(p, true);
                    }

                    testPath($"{pathStr}.core.stream");

                    foreach (string ext in Packfile.ValidFileExtensions)
                        testPath($"{pathStr}{ext}");

                    foreach (string lang in possibleLanguages)
                        testPath($"{pathStr}.{lang}.stream");
                };

                File.WriteAllLines(options.OutputPath, allValidPaths.Keys.OrderBy(x => x));
                Console.WriteLine($"Total possible files in archives: {device.ActiveFiles.Count}");
                Console.WriteLine($"Total valid files: {allValidPaths.Count}");
            }
            else
            {
                File.WriteAllLines(options.OutputPath, allStrings.Keys.OrderBy(x => x));
                Console.WriteLine($"Total lines extracted: {allStrings.Count}");
            }
        }

        public static void ExportIndexFiles(ExportIndexFilesCommand options)
        {
            var sourceIndexes = Util.GatherFiles(options.InputPath, new[] { ".idx" }, out _);
            var allValidPaths = new ConcurrentDictionary<string, bool>();

            foreach (var (indexPath, _) in sourceIndexes)
            {
                var packfileIndex = PackfileIndex.FromFile(indexPath);

                foreach (string corePath in packfileIndex.Entries.Select(x => x.FilePath))
                {
                    string pathStr = Util.RemoveMountPrefixes(corePath.ToLower());
                    allValidPaths.TryAdd(pathStr, true);
                }
            }

            File.WriteAllLines(options.OutputPath, allValidPaths.Keys.OrderBy(x => x));
        }
    }
}
