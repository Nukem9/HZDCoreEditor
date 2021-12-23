namespace HZDCoreTools;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using CommandLine;
using Decima;

public static class Archive
{
    private const string PrefetchCorePath = "prefetch/fullgame.prefetch.core";

    public class ArchiveCommand
    {
        [Option('r', "ignore", HelpText = "If specified, use this regex to exclude files matching the filter.")]
        public string IgnoredRegex { get; set; }

        [Option('v', "verbose", HelpText = "Print extra information to the console, such as files being extracted.")]
        public bool Verbose { get; set; }
    }

    [Verb("pack", HelpText = "Create a game archive.")]
    public class PackArchiveCommand : ArchiveCommand
    {
        [Option('i', "input", Required = true, HelpText = "OS input path for game data (*.*, .core, .stream). Wildcards (*) supported.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "OS output path for the generated bin file (.bin).")]
        public string OutputPath { get; set; }

        [Option('f', "force", HelpText = "Force include unsupported file extensions.")]
        public bool ForceUnsupported { get; set; }
    }

    [Verb("unpack", HelpText = "Extract a game archive.")]
    public class ExtractArchiveCommand : ArchiveCommand
    {
        [Option('i', "input", Required = true, HelpText = "OS input path for game data (.bin). Wildcards (*) supported.")]
        public string InputPath { get; set; }

        [Option('o', "output", Required = true, HelpText = "OS output directory for core files.")]
        public string OutputPath { get; set; }
    }

    public static void PackArchive(PackArchiveCommand options)
    {
        var sourceFiles = Util.GatherFiles(options.InputPath, null, out string _);
        var ignoredFileFilter = string.IsNullOrEmpty(options.IgnoredRegex) ? null : new Regex(options.IgnoredRegex);

        IEnumerable<string> GatherValidFiles()
        {
            foreach ((string absolute, string relative) in sourceFiles)
            {
                string sanitized = Packfile.SanitizePath(relative, string.Empty);

                if (!options.ForceUnsupported)
                {
                    string ext = Path.GetExtension(sanitized);

                    if (!Packfile.ValidFileExtensions.Any(x => x.Equals(ext)))
                    {
                        Console.WriteLine($"Skipping '{sanitized}' because it has an invalid extension");
                        continue;
                    }
                }

                if (ignoredFileFilter?.IsMatch(sanitized) ?? false)
                {
                    if (options.Verbose)
                        Console.WriteLine($"Skipping '{sanitized}' because it was filtered");

                    continue;
                }

                yield return sanitized;
            }
        }

        var packfile = new PackfileWriter(options.OutputPath, false, FileMode.Create);
        packfile.BuildFromFileList(options.InputPath, GatherValidFiles());
    }

    public static void ExtractArchive(ExtractArchiveCommand options)
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

        // Dump load order
        if (options.Verbose)
        {
            var loadOrder = device.ActiveArchives.ToList();

            for (int i = 0; i < loadOrder.Count; i++)
                Console.WriteLine($"Using '{loadOrder[i]}' with priority {i}");
        }

        // Extract all files where a name can be found
        var ignoredFileFilter = string.IsNullOrEmpty(options.IgnoredRegex) ? null : new Regex(options.IgnoredRegex);
        var prefetchNames = BuildFileNamesFromPrefetch(device);
        int filesExtracted = 0;

        Console.WriteLine("Prefetch file names available: {0}", prefetchNames.Count > 0 ? "Yes" : "No");
        Console.WriteLine($"Total files stored in archives: {device.ActiveFiles.Count}");

        device.ActiveFiles.AsParallel().ForAll(file =>
        {
            if (!device.PathIdToFileName(file, out string corePath))
            {
                if (!prefetchNames.TryGetValue(file, out corePath))
                {
                    if (options.Verbose)
                        Console.WriteLine($"Skipping '{file:X16}' because a name is not available");

                    return;
                }
            }

            if (ignoredFileFilter?.IsMatch(corePath) ?? false)
            {
                if (options.Verbose)
                    Console.WriteLine($"Skipping '{corePath}' because it was filtered");

                return;
            }

            // PathIdToFileName or prefetchNames are expected to resolve all names by themselves. Don't do any extra processing here.
            string diskFilePath = Path.Combine(options.OutputPath, corePath);

            if (options.Verbose)
                Console.WriteLine($"Extracting '{corePath}...");

            Directory.CreateDirectory(Path.GetDirectoryName(diskFilePath));
            device.ExtractFile(corePath, diskFilePath, FileMode.Create);

            Interlocked.Increment(ref filesExtracted);
        });

        Console.WriteLine($"Total files extracted: {filesExtracted}");
    }

    public static Dictionary<ulong, string> BuildFileNamesFromPrefetch(PackfileDevice device)
    {
        var lookupTable = new Dictionary<ulong, string>();

        // No prefetch present -> return nothing
        if (device.HasFile(PrefetchCorePath))
        {
            var prefetchCore = Util.ExtractCoreBinaryInMemory(device, PrefetchCorePath);
            var prefetch = prefetchCore.Objects.OfType<Decima.HZD.PrefetchList>().Single();

            foreach (var file in prefetch.Files.Select(x => x.Path))
            {
                // .core
                string coreOnly = Packfile.SanitizePath(file, Packfile.CoreExt);
                lookupTable.TryAdd(Packfile.GetHashForPath(coreOnly), coreOnly);

                // .core.stream
                string coreStreamOnly = Packfile.SanitizePath(file, Packfile.StreamExt);
                lookupTable.TryAdd(Packfile.GetHashForPath(coreStreamOnly), coreStreamOnly);
            }
        }

        return lookupTable;
    }
}
