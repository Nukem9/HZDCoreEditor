namespace HZDCoreTools;

using CommandLine;
using CommandLine.Text;
using Decima;
using HZDCoreTools.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class ArchiveIndex
{
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

    public static void ExportIndexFiles(ExportIndexFilesCommand options)
    {
        var sourceIndexes = Utils.GatherFiles(options.InputPath, new[] { ".idx" }, out _);
        var allValidPaths = new ConcurrentDictionary<string, bool>();

        foreach (var (indexPath, _) in sourceIndexes)
        {
            var packfileIndex = PackfileIndex.FromFile(indexPath);

            foreach (string corePath in packfileIndex.Entries.Select(x => x.FilePath))
            {
                string pathStr = Utils.RemoveMountPrefixes(corePath.ToLower());
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
        var sourceArchives = Utils.GatherFiles(options.InputPath, new[] { ".bin" }, out _);

        foreach ((string absolutePath, string relativePath) in sourceArchives)
        {
            Console.WriteLine($"Processing {relativePath}...");

            using var archive = new PackfileReader(absolutePath);
            var index = PackfileIndex.RebuildFromArchive(archive, lookupTable);

            if (archive.FileEntries.Count != index.Entries.Count)
                Console.WriteLine($"Possible entries: {archive.FileEntries.Count} Mapped entries: {index.Entries.Count}");

            index.ToFile(Path.ChangeExtension(absolutePath, ".idx"), FileMode.Create);
        }
    }
}