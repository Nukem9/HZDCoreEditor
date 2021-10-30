using CommandLine;
using CommandLine.Text;
using Decima;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HZDCoreTools
{
    public static class Misc
    {
        private const string PrefetchCorePath = "prefetch/fullgame.prefetch.core";

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
                    yield return new Example("Extract single index", new ExportIndexFilesCommand
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

                    yield return new Example("Update single bin", new RebuildIndexFilesCommand
                    {
                        InputPath = @"E:\HZD\Packed_DX12\DLC1.bin",
                        OutputPath = @"E:\HZD\Packed_DX12\",
                        LookupFile = "DLC1_file_lines.txt",
                    });
                }
            }
        }

        [Verb("rebuildprefetch", HelpText = "Rebuild fullgame.prefetch.core from a set of archives.")]
        public class RebuildPrefetchFileCommand
        {
            [Option('i', "input", Required = true, HelpText = "OS input path for game data (.bin). Wildcards (*) supported.")]
            public string InputPath { get; set; }

            [Option('o', "output", Required = true, HelpText = "OS output path for the generated core or archive file (.bin, .core).")]
            public string OutputPath { get; set; }

            [Option("skipsizes", HelpText = "Skip rebuilding of file sizes.")]
            public bool SkipSizes { get; set; }

            [Option("skiplinks", HelpText = "Skip rebuilding of ref links.")]
            public bool SkipLinks { get; set; }

            [Option('v', "verbose", HelpText = "Print extra information to the console, such as which entries are updated.")]
            public bool Verbose { get; set; }

            [Usage(ApplicationAlias = nameof(HZDCoreTools))]
            public static IEnumerable<Example> Examples
            {
                get
                {
                    yield return new Example("Update all to archive", new RebuildPrefetchFileCommand
                    {
                        InputPath = @"E:\HZD\Packed_DX12\*.bin",
                        OutputPath = @"E:\HZD\Packed_DX12\Patch_ZPrefetch.bin",
                    });

                    yield return new Example("Update all to core", new RebuildPrefetchFileCommand
                    {
                        InputPath = @"E:\HZD\Packed_DX12\*.bin",
                        OutputPath = @"E:\HZD\Packed_DX12\extracted\prefetch\fullgame.prefetch.core",
                        SkipLinks = true,
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
                // There's no way to determine if a file is a .core or a .stream.core. This might throw an exception.
                try
                {
                    var coreBinary = Util.ExtractCoreBinaryInMemory(device, fileId);

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

        public static void RebuildPrefetchFile(RebuildPrefetchFileCommand options)
        {
            var sourceFiles = Util.GatherFiles(options.InputPath, new[] { ".bin" }, out string _);

            // Add each bin
            using var device = new PackfileDevice();

            foreach ((string absolute, _) in sourceFiles)
            {
                if (!device.Mount(absolute))
                {
                    Console.WriteLine($"Unable to mount '{absolute}'");
                    return;
                }
            }

            if (!device.HasFile(PrefetchCorePath))
                throw new FileNotFoundException($"'{PrefetchCorePath}' not found. Expected at least one archive to have an existing prefecth core file.");

            // Extract the .core, iterate over each file, then update the size for each file
            var prefetchCore = Util.ExtractCoreBinaryInMemory(device, PrefetchCorePath);
            var prefetch = prefetchCore.Objects.OfType<Decima.HZD.PrefetchList>().Single();

            if (prefetch.Files.Count != prefetch.Sizes.Count)
                throw new Exception("Prefetch 'Files' and 'Sizes' array lengths don't match?!");

            RebuildPrefetchForFiles(prefetch, device, options);

            if (Path.GetExtension(options.OutputPath) == ".bin")
            {
                // Pack it into a new archive
                using var ms = new MemoryStream();
                prefetchCore.ToData(new BinaryWriter(ms));

                ms.Position = 0;
                var streamList = new List<(string, Stream)>
                {
                    (PrefetchCorePath, ms),
                };

                using var packfile = new PackfileWriter(options.OutputPath, false, FileMode.Create);
                packfile.BuildFromStreamList(streamList);
            }
            else
            {
                // Straight to disk
                prefetchCore.ToFile(options.OutputPath, FileMode.Create);
            }
        }

        private static void RebuildPrefetchForFiles(Decima.HZD.PrefetchList prefetch, PackfileDevice device, RebuildPrefetchFileCommand options)
        {
            // Convert the old links to a dictionary
            var links = new ConcurrentDictionary<int, int[]>();
            int linkIndex = 0;

            for (int i = 0; i < prefetch.Files.Count; i++)
            {
                int count = prefetch.Links[linkIndex];

                var indices = prefetch.Links
                    .Skip(linkIndex + 1)
                    .Take(count)
                    .ToArray();

                links.TryAdd(i, indices);
                linkIndex += count + 1;
            }

            // Create a lookup table to map a file name to an index
            var fileIndexLookup = new Dictionary<string, int>();

            for (int i = 0; i < prefetch.Files.Count; i++)
                fileIndexLookup.TryAdd(prefetch.Files[i].Path, i);

            int getFileIndex(string path)
            {
                if (fileIndexLookup.TryGetValue(path, out int index))
                    return index;

                throw new FileNotFoundException("Core path not found", path);
            }

            // Foreach (core in parallel)
            Parallel.ForEach(prefetch.Files, (assetFile, _, index) =>
            {
                int i = (int)index;
                string corePath = Packfile.SanitizePath(assetFile.Path);

                if (!device.HasFile(corePath))
                    return;

                // Rebuild sizes
                if (!options.SkipSizes)
                {
                    int binarySize = (int)device.GetFileSize(corePath);

                    if (prefetch.Sizes[i] != binarySize)
                    {
                        if (options.Verbose)
                            Console.WriteLine($"Updating size for '{corePath}' ({prefetch.Sizes[i]} != {binarySize})");

                        prefetch.Sizes[i] = binarySize;
                    }
                }

                // Rebuild references (don't forget to remove duplicates (Distinct()!))
                if (!options.SkipLinks)
                {
                    var coreBinary = Util.ExtractCoreBinaryInMemory(device, corePath);

                    // TODO: Skip or fix unknown CoreBinary types. If a type is unknown, it'll return 0 references. The game seems
                    // to be okay with it as-is. The original prefetech core is generated by a dev tool that I don't have.
                    var newLinks = coreBinary.GetAllReferences()
                        .Where(x => x.Type == BaseRef.Types.ExternalLink)
                        .Select(x => getFileIndex(x.ExternalFile))
                        .Distinct()
                        .OrderBy(x => x)
                        .ToArray();

                    if (!links.TryGetValue(i, out int[] oldLinks))
                        throw new KeyNotFoundException("Failed to get a value? Previous code guarantees this shouldn't happen.");

                    // Element order doesn't matter. Only check the sets for equality.
                    if (!oldLinks.OrderBy(x => x).SequenceEqual(newLinks))
                    {
                        if (options.Verbose)
                            Console.WriteLine($"Updating links for '{corePath}' ({oldLinks.Length} != {newLinks.Length})");

                        links[i] = newLinks;
                    }
                }
            });

            // Dictionary of links -> linear array
            prefetch.Links.Clear();

            for (int i = 0; i < prefetch.Files.Count; i++)
            {
                var indices = links[i];

                prefetch.Links.Add(indices.Length);
                prefetch.Links.AddRange(indices);
            }
        }

        private static void DumpPrefetchLinksToFile(Decima.HZD.PrefetchList prefetch, string filePath)
        {
            var allLines = new List<string>();
            int linkIndex = 0;

            for (int i = 0; i < prefetch.Files.Count; i++)
            {
                string path = prefetch.Files[i].Path;
                int count = prefetch.Links[linkIndex];

                var indices = prefetch.Links
                    .Skip(linkIndex + 1)
                    .Take(count);

                linkIndex += count + 1;

                allLines.Add($"{i} '{path}': ");
                foreach (int index in indices)
                    allLines.Add($"\t{index} => '{prefetch.Files[index].Path}'");
            }

            File.WriteAllLines(filePath, allLines);
        }
    }
}