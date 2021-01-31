using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Decima;
using Decima.HZD;
using HZDCoreEditor.Util;
using String = System.String;

namespace HZDCoreTools
{
    public class Extractor
    {
        private const string Prefetch = "prefetch/fullgame.prefetch.core";
        private readonly List<string> GameFiles = new List<string>()
        {
            "DLC1.bin",
            "DLC1_English.bin",
            "FGRWin32.bin",
            "Initial.bin",
            "Initial_English.bin",
            "Remainder.bin",
            "Remainder_English.bin",
            "Patch.bin"
        };
        private readonly string[] Ignored =
        {
            "sounds",
        };

        public string OutputDir { get; set; }

        public void Extract(string path, bool game)
        {
            if (File.Exists(path))
                ExtractFile(path);
            else if (Directory.Exists(path))
                ExtractDir(path, game);
            else
            {
                Console.WriteLine("Error, path not found: " + path);
            }
        }

        private void ExtractFile(string path)
        {
            var pack = new PackfileReader(path);
            var prefetchHash = Packfile.GetHashForPath(Prefetch);
            var hasPrefetch = pack.FileEntries.Any(x => x.PathHash == prefetchHash);

            if (!hasPrefetch)
            {
                if (!Confirm("Prefetch not found, file names cannot be extracted."))
                    return;

                CheckDirectory(OutputDir);

                Console.WriteLine($"Starting extracting {pack.FileEntries.Count} files");
                var tasks = new ParallelTasks<(ulong Hash, string Output)>(
                    Environment.ProcessorCount, data => pack.ExtractFile(data.Hash, data.Output));
                tasks.Start();
                pack.FileEntries.ForEach(x => tasks.AddItem((x.PathHash, Path.Combine(OutputDir, $"{x.PathHash}.core"))));
                tasks.WaitForComplete();
                Console.WriteLine("Extraction complete");
                return;
            }

            var prefetch = LoadPrefetch(pack);
            var files = pack.FileEntries.ToDictionary(x => x.PathHash, x => pack);
            ExtractWithPrefetch(prefetch, files);
        }

        private int progressValue = 0;
        private void ExtractDir(string path, bool game)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine("Error, directory not found: " + path);
                return;
            }

            var prefetchHash = Packfile.GetHashForPath(Prefetch);

            var packs = game ? GetGameFiles(path) : Directory.GetFiles(path);
            var files = BuildPackMap(packs);
            if (!files.ContainsKey(prefetchHash))
            {
                Console.WriteLine("Prefetch not found, directory cannot be extracted");
                return;
            }

            var prefetch = LoadPrefetch(files[prefetchHash]);
            ExtractWithPrefetch(prefetch, files);
        }

        private void ExtractWithPrefetch(List<string> prefetch, Dictionary<ulong, PackfileReader> files)
        {

            CheckDirectory(OutputDir);

            Console.WriteLine($"Starting extracting {prefetch.Count} files");
            using var progressBar = new ProgressBar();
            progressValue = 0;
            int lastProgress = 0;
            progressBar.Report(0);
            var tasks = new ParallelTasks<string>(
                Environment.ProcessorCount, file =>
                {
                    var hash = Packfile.GetHashForPath(file);
                    var output = Path.Combine(OutputDir, file + ".core");
                    if (files.TryGetValue(hash, out var pack))
                    {
                        CheckDirectory(Path.GetDirectoryName(output));

                        Interlocked.Increment(ref progressValue);
                        var val = (int)((progressValue * 1.0 / prefetch.Count) * 100);
                        if (val > lastProgress)
                        {
                            lastProgress = (int)val;
                            progressBar.Report(val / 100.0);
                        }

                        pack.ExtractFile(hash, output, true);
                    }
                });

            tasks.Start();
            foreach (var x in prefetch.Where(x => !Ignored.Any(x.StartsWith)))
                tasks.AddItem(x);
            tasks.WaitForComplete();
            progressBar.Report(1);
            Console.WriteLine("Extraction complete");
        }

        private Dictionary<ulong, PackfileReader> BuildPackMap(string[] packFiles)
        {
            var files = new Dictionary<ulong, PackfileReader>();

            foreach (var packFile in packFiles)
            {
                var pack = new PackfileReader(packFile);
                for (int i = 0; i < pack.FileEntries.Count; i++)
                {
                    var hash = pack.FileEntries[i].PathHash;
                    files[hash] = pack;
                }
            }

            return files;
        }

        private string[] GetGameFiles(string dir)
        {
            var files = Directory.GetFiles(dir)
                .Where(x => GameFiles.Contains(Path.GetFileName(x)))
                .OrderBy(x => GameFiles.IndexOf(Path.GetFileName(x)))
                .ToArray();

            return files;
        }

        private List<string> LoadPrefetch(PackfileReader pack)
        {
            var prefetchHash = Packfile.GetHashForPath(Prefetch);
            
            using var ms = new MemoryStream();
            pack.ExtractFile(prefetchHash, ms);
            ms.Position = 0;

            using var br = new BinaryReader(ms, Encoding.UTF8, true);
            var core = CoreBinary.FromData(br, true);

            return (core.First(x => x is PrefetchList) as PrefetchList).Files
                .Select(x=>x.Path?.Value)
                .ToList();
        }

        private bool Confirm(string message)
        {
            Console.WriteLine(message);
            return true;
        }

        public static void CheckDirectory(string dir)
        {
            if (String.IsNullOrEmpty(dir))
                return;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}
