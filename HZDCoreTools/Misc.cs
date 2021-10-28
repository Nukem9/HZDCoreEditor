using CommandLine;
using CommandLine.Text;
using Decima;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HZDCoreTools
{
    public static class Misc
    {
        [Verb("exportstrings", HelpText = "Extract all strings contained in a set of archives.")]
        public class ExportAllStringsCommand
        {
            [Option('i', "input", Required = true, HelpText = "OS input path for game data (.bin). Wildcards (*) supported.")]
            public string InputPath { get; set; }

            [Option('o', "output", Required = true, HelpText = "OS output path for the generated text file (.txt, *.*).")]
            public string OutputPath { get; set; }

            [Option("validpathsonly", HelpText = "Only dump strings that contain valid core file paths.")]
            public bool ValidPathsOnly { get; set; }

            [Usage(ApplicationAlias = nameof(HZDCoreTools))]
            public static IEnumerable<Example> Examples
            {
                get
                {
                    yield return new Example("Extract all", new ExportAllStringsCommand
                    {
                        InputPath = @"E:\HZD\Packed_DX12\*.bin",
                        OutputPath = @"E:\HZD\Packed_DX12\valid_file_lines.txt",
                        ValidPathsOnly = true,
                    });
                }
            }
        }

        [Verb("exportindexstrings", HelpText = "Extract all paths contained in a set of archive index files.")]
        public class ExportIndexFilesCommand
        {
            [Option('i', "input", Required = true, HelpText = "OS input path for game data (.idx). Wildcards (*) supported.")]
            public string InputPath { get; set; }

            [Option('o', "output", Required = true, HelpText = "OS output path for the generated text file (.txt, *.*).")]
            public string OutputPath { get; set; }

            [Usage(ApplicationAlias = nameof(HZDCoreTools))]
            public static IEnumerable<Example> Examples
            {
                get
                {
                    yield return new Example("Update all", new ExportIndexFilesCommand
                    {
                        InputPath = @"E:\HZD\Packed_DX12\Initial.idx",
                        OutputPath = @"E:\HZD\Packed_DX12\valid_file_lines.txt",
                    });
                }
            }
        }

        [Verb("rebuildindexfiles", HelpText = "Rebuild index files from a set of archives.")]
        public class RebuildIndexFilesCommand
        {
            [Option('i', "input", Required = true, HelpText = "OS input path for game data (.bin). Wildcards (*) supported.")]
            public string InputPath { get; set; }

            [Option('o', "output", Required = true, HelpText = "OS output directory for the generated index files (.idx).")]
            public string OutputPath { get; set; }

            [Option('l', "lookupfile", Required = true, HelpText = "OS input path for a text file containing possible core file paths (.txt, *.*).")]
            public string LookupFile { get; set; }

            [Usage(ApplicationAlias = nameof(HZDCoreTools))]
            public static IEnumerable<Example> Examples
            {
                get
                {
                    yield return new Example("Update all", new RebuildIndexFilesCommand
                    {
                        InputPath = @"E:\HZD\Packed_DX12\*.bin",
                        OutputPath = @"E:\HZD\Packed_DX12\",
                        LookupFile = "valid_file_lines.txt",
                    });

                    yield return new Example("Update single file", new RebuildIndexFilesCommand
                    {
                        InputPath = @"E:\HZD\Packed_DX12\DLC1.bin",
                        OutputPath = @"E:\HZD\Packed_DX12\",
                        LookupFile = "valid_file_lines.txt",
                    });
                }
            }
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
                using var stream = new MemoryStream();
                device.ExtractFile(fileId, stream);

                // There's no way to determine if a file is a .core or a .stream.core. This might throw an exception.
                try
                {
                    stream.Position = 0;
                    var coreBinary = CoreBinary.FromData(new BinaryReader(stream));

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
                var allValidPaths = new ConcurrentDictionary<string, bool>();
                var possibleLanguages = Enum.GetNames(typeof(Decima.HZD.ELanguage))
                    .Select(x => x.ToLower())
                    .ToArray();

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
                    // path/to/file.<language>.stream
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

        public static void RebuildIndexFiles(RebuildIndexFilesCommand options)
        {
            // Create table of lookup strings
            var fileLines = File.ReadAllLines(options.LookupFile);
            var lookupTable = new Dictionary<ulong, string>();

            foreach (string line in fileLines)
                lookupTable.TryAdd(Packfile.GetHashForPath(line), line);

            // Then apply them to the bins
            var sourceArchives = Util.GatherFiles(options.InputPath, new[] { ".bin" }, out _);

            foreach ((string absolutePath, string relativePath) in sourceArchives)
            {
                Console.WriteLine($"Processing {relativePath}...");

                using var archive = new PackfileReader(absolutePath);
                var index = PackfileIndex.RebuildFromArchive(archive, lookupTable);

                index.ToFile(Path.ChangeExtension(absolutePath, ".idx"), FileMode.Create);
            }
        }
    }
}
