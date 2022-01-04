namespace HZDCoreTools;

using CommandLine;
using CommandLine.Text;
using Decima;
using HZDCoreTools.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    [Verb("exportentrypointnames", HelpText = "Extract all script entry point names contained in a set of archives.")]
    public class ExportEntryPointNamesCommand
    {
        [Option('i', "input", Required = true, HelpText = "OS input path for game data (.bin). Wildcards (*) supported.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "OS output path for the generated text file (.txt, *.*).")]
        public string OutputPath { get; set; }

        [Usage(ApplicationAlias = nameof(HZDCoreTools))]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Extract all", new ExportAllStringsCommand
                {
                    InputPath = @"E:\HZD\Packed_DX12\*.bin",
                    OutputPath = @"E:\HZD\Packed_DX12\valid_entrypoints.txt",
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

        [Option("patchesonly", HelpText = "Skip all archives except for patch files.")]
        public bool PatchesOnly { get; set; }

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

    [Verb("coretojson", HelpText = "Convert binary core files to json format.")]
    public class CoreToJsonCommand
    {
        [Option('i', "input", Required = true, HelpText = "OS input path for game data (*.core). Wildcards (*) supported.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "OS output directory for the generated json files (.json).")]
        public string OutputPath { get; set; }
    }

    [Verb("jsontocore", HelpText = "Convert json core files to binary format.")]
    public class JsonToCoreCommand
    {
        [Option('i', "input", Required = true, HelpText = "OS input path for game data (*.json). Wildcards (*) supported.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "OS output directory for the generated core files (.core).")]
        public string OutputPath { get; set; }
    }

    public static void ExportAllStrings(ExportAllStringsCommand options)
    {
        var sourceBins = Utils.GatherFiles(options.InputPath, new[] { ".bin" }, out _);

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
                var coreBinary = Utils.ExtractCoreBinaryInMemory(device, fileId);

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
            var allValidPaths = new Dictionary<string, bool>();
            var possibleLanguages = Enum.GetNames(typeof(Decima.HZD.ELanguage))
                .Concat(Enum.GetNames(typeof(Decima.DS.ELanguage)))
                .Select(x => x.ToLower())
                .Distinct()
                .ToArray();

            foreach (string key in allStrings.Keys)
            {
                string pathStr = Utils.RemoveMountPrefixes(key);
                pathStr = pathStr.Replace(Packfile.StreamExt, string.Empty);
                pathStr = pathStr.Replace(Packfile.CoreExt, string.Empty);

                // Possible extensions to check for:
                // path/to/file.core
                // path/to/file.coredebug
                // path/to/file.coretext
                // path/to/file.dep
                // path/to/file.core.stream
                // path/to/file.<language>.stream
                // path/to/file.wem.<language>.core.stream
                void TestPath(string p)
                {
                    if (device.HasFile(p))
                        allValidPaths.TryAdd(p, true);
                }

                TestPath($"{pathStr}.core.stream");

                foreach (string ext in Packfile.ValidFileExtensions)
                    TestPath($"{pathStr}{ext}");

                foreach (string lang in possibleLanguages)
                {
                    TestPath($"{pathStr}.{lang}.stream");
                    TestPath($"{pathStr}.wem.{lang}.core.stream");
                }
            }

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

    public static void ExportEntryPointNames(ExportEntryPointNamesCommand options)
    {
        var sourceBins = Utils.GatherFiles(options.InputPath, new[] { ".bin" }, out _);

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
                var coreBinary = Utils.ExtractCoreBinaryInMemory(device, fileId);

                coreBinary.VisitAllObjects((Decima.DS.ProgramResourceEntryPoint entryPoint, object _) =>
                {
                    uint nameChecksum = CRC32C.Checksum(Encoding.UTF8.GetBytes(entryPoint.EntryPoint)) & ~0x80000000u;

                    allStrings.TryAdd($"0x{nameChecksum:X},{entryPoint.EntryPoint}", true);
                });
            }
            catch (InvalidDataException)
            {
            }
            catch (EndOfStreamException)
            {
            }
        });

        File.WriteAllLines(options.OutputPath, allStrings.Keys.OrderBy(x => x));
        Console.WriteLine($"Total lines extracted: {allStrings.Count}");
    }

    public static void RebuildPrefetchFile(RebuildPrefetchFileCommand options)
    {
        CoreBinary prefetchCore = null;

        using (var device = new PackfileDevice())
        {
            // Add each bin
            var sourceFiles = Utils.GatherFiles(options.InputPath, new[] { ".bin" }, out string _);

            foreach ((string absolute, string relative) in sourceFiles)
            {
                if (options.PatchesOnly)
                {
                    if (!relative.Contains("patch", StringComparison.InvariantCultureIgnoreCase))
                        continue;
                }

                if (!device.Mount(absolute))
                {
                    Console.WriteLine($"Unable to mount '{absolute}'");
                    return;
                }
            }

            if (!device.HasFile(PrefetchCorePath))
                throw new FileNotFoundException($"'{PrefetchCorePath}' not found. Expected at least one archive to have an existing prefecth core file.");

            // Extract the .core, iterate over each file, then update the size for each file
            prefetchCore = Utils.ExtractCoreBinaryInMemory(device, PrefetchCorePath);
            var prefetch = prefetchCore.Objects.OfType<Decima.HZD.PrefetchList>().Single();

            if (prefetch.Files.Count != prefetch.Sizes.Count)
                throw new Exception("Prefetch 'Files' and 'Sizes' array lengths don't match?!");

            RebuildPrefetchForFiles(prefetch, device, options);
        }

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

        int GetFileIndex(string path)
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
                var coreBinary = Utils.ExtractCoreBinaryInMemory(device, corePath);

                // TODO: Skip or fix unknown CoreBinary types. If a type is unknown, it'll return 0 references. The game seems
                // to be okay with it as-is. The original prefetech core is generated by a dev tool that I don't have.
                var newLinks = coreBinary.GetAllReferences()
                    .Where(x => x.Type == BaseRef.Types.ExternalLink)
                    .Select(x => GetFileIndex(x.ExternalFile))
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

    public static void CoreToJson(CoreToJsonCommand options)
    {
        var sourceCores = Utils.GatherFiles(options.InputPath, new[] { ".core" }, out _);
        var serializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new List<JsonConverter>() { new BaseGGUUIDConverter() },
        };

        foreach ((string absolute, string relative) in sourceCores)
        {
            var core = CoreBinary.FromFile(absolute);
            var outputJsonPath = Path.Combine(options.OutputPath, relative) + ".json";

            Console.WriteLine($"Converting {relative}...");

            Directory.CreateDirectory(Path.GetDirectoryName(outputJsonPath));
            File.WriteAllText(outputJsonPath, JsonConvert.SerializeObject(core.Objects, serializerSettings));
        }
    }

    public static void JsonToCore(JsonToCoreCommand options)
    {
        var sourceJsons = Utils.GatherFiles(options.InputPath, new[] { ".json" }, out _);
        var serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = new List<JsonConverter>() { new BaseGGUUIDConverter() },
        };

        foreach ((string absolute, string relative) in sourceJsons)
        {
            var objects = JsonConvert.DeserializeObject<IEnumerable<object>>(File.ReadAllText(absolute), serializerSettings);
            var core = new CoreBinary();
            var outputCorePath = Path.Combine(options.OutputPath, Path.ChangeExtension(relative, ".core"));

            Console.WriteLine($"Converting {relative}...");

            foreach (var obj in objects)
                core.AddObject(obj);

            Directory.CreateDirectory(Path.GetDirectoryName(outputCorePath));
            core.ToFile(outputCorePath, FileMode.Create);
        }
    }
}
