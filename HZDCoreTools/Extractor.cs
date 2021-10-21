using Decima;
using Decima.HZD;
using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        private List<Regex> Ignored = new List<Regex>();

        public string OutputDir { get; set; }

        public void Extract(CmdOptions options)
        {
            if (!String.IsNullOrEmpty(options.Ignore))
                Ignored.Add(new Regex(options.Ignore));

            if (File.Exists(options.ExtractPath))
                ExtractFile(options.ExtractPath);
            else if (Directory.Exists(options.ExtractPath))
                ExtractDir(options.ExtractPath, options.GameDir, options.Streams);
            else
            {
                Console.WriteLine("Error, path not found: " + options.ExtractPath);
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
                foreach (var x in pack.FileEntries)
                    tasks.AddItem((x.PathHash, Path.Combine(OutputDir, $"{x.PathHash}.core")));
                tasks.WaitForComplete();
                Console.WriteLine("");
                Console.WriteLine("Extraction complete");
                return;
            }

            var prefetch = LoadPrefetch(pack);
            var files = pack.FileEntries.ToDictionary(x => x.PathHash, x => pack);
            ExtractWithPrefetch(prefetch, files);
        }

        private int progressValue = 0;
        private void ExtractDir(string path, bool game, bool streams)
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
            ExtractWithPrefetch(prefetch, files, streams);
        }

        private void ExtractWithPrefetch(List<string> prefetch, Dictionary<ulong, PackfileReader> files, bool streams = false)
        {
            CheckDirectory(OutputDir);

            Console.WriteLine($"Starting extracting {prefetch.Count} files");
            using (var progressBar = new ProgressBar())
            {
                progressValue = 0;
                int lastProgress = 0;
                progressBar.Report(0);
                var tasks = new ParallelTasks<string>(
                    Environment.ProcessorCount, file =>
                    {
                        void extractFromPack(string fileName, bool stream)
                        {
                            var hash = Packfile.GetHashForPath(Packfile.SanitizePath(fileName, Packfile.StreamExt));
                            var output = Path.Combine(OutputDir, fileName);
                            if (files.TryGetValue(hash, out var pack))
                            {
                                CheckDirectory(Path.GetDirectoryName(output));
                                pack.ExtractFile(hash, output, true);
                            }
                        }

                        extractFromPack(file + ".core", false);
                        if (streams)
                            extractFromPack(file + ".core.stream", true);

                        Interlocked.Increment(ref progressValue);
                        var val = (int)((progressValue * 1.0 / prefetch.Count) * 100);
                        if (val > lastProgress)
                        {
                            lastProgress = (int)val;
                            progressBar.Report(val / 100.0);
                        }
                    });

                tasks.Start();
                foreach (var file in prefetch.Where(x => !Ignored.Any(i => i.IsMatch(x))))
                    tasks.AddItem(file);
                tasks.WaitForComplete();
            }
            Console.WriteLine("");
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
                .Select(x => x.Path?.Value)
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

        public void ExtractHZDLocalization()
        {
            var files = Directory.GetFiles(Path.Combine(OutputDir, @"localized\"), "*.core", SearchOption.AllDirectories);
            var sb = new StringBuilder();

            foreach (string file in files)
            {
                Console.WriteLine(file);
                bool first = true;

                var core = CoreBinary.FromFile(file);

                foreach (var obj in core)
                {
                    if (obj is Decima.HZD.LocalizedTextResource asResource)
                    {
                        if (first)
                        {
                            sb.AppendLine();
                            sb.AppendLine(file);
                            first = false;
                        }

                        sb.AppendLine(asResource.ObjectUUID + " " + asResource.GetStringForLanguage(Decima.HZD.ELanguage.English));
                    }
                }
            }

            File.WriteAllText(Path.Combine(OutputDir, @"text_data_dump.txt"), sb.ToString());
        }
    }
}
