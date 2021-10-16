using HZDCoreEditor.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HZDCoreSearch
{
    public class ByteSearchResult<T>
    {
        public string File { get; }
        public List<(T Key, int[] Positions)> Results { get; }
        public bool Found => Results.Any();

        public ByteSearchResult(string file, List<(T Key, int[] Positions)> results)
        {
            File = file;
            Results = results;
        }
    }

    public class ByteSearch
    {
        private ParallelTasks<string> _searchTasks;

        public string Dir { get; }
        public HashSet<string> IgnoredFiles { get; }

        public ByteSearch(string dir, IEnumerable<string> ignoredFiles = null)
        {
            Dir = dir;
            IgnoredFiles = ignoredFiles == null ? null : new HashSet<string>(ignoredFiles, StringComparer.OrdinalIgnoreCase);
        }

        public async Task SearchAsync<T>(
            List<(T Key, byte[] Data)> byteSearches,
            Action<ByteSearchResult<T>> fileSearched)
        {
            await Task.Run(() =>
            {
                Search(byteSearches, fileSearched);
            });
        }
        public void Search<T>(
            List<(T Key, byte[] Data)> byteSearches,
            Action<ByteSearchResult<T>> fileSearched)
        {
            try
            {
                SearchDirs(Dir, byteSearches, fileSearched);
            }
            catch (Exception ex)
            {
                if (!(ex?.InnerException is OperationCanceledException))
                    throw;
            }

            _searchTasks = null;
        }

        public void WaitForComplete()
        {
            _searchTasks?.WaitForComplete();
        }

        public void Stop()
        {
            if (_searchTasks == null)
                return;

            _searchTasks.Stop();
            _searchTasks.WaitForComplete();
        }


        private void SearchDirs<T>(string dir, List<(T Key, byte[] Data)> patterns,
            Action<ByteSearchResult<T>> fileSearched)
        {
            var bm = patterns.Select(x => new BoyerMoore(x.Data)).ToList();

            _searchTasks = new ParallelTasks<string>(Environment.ProcessorCount, f =>
            {
                var data = File.ReadAllBytes(f);

                var results = new List<(T Key, int[] Positions)>();
                for (int i = 0; i < patterns.Count; i++)
                {
                    var pos = bm[i].SearchAll(data);
                    if (pos.Any())
                    {
                        results.Add((patterns[i].Key, pos.ToArray()));
                    }
                }

                fileSearched(new ByteSearchResult<T>(f, results));
            });

            _searchTasks.Start();

            RecurseDirs(dir, f => _searchTasks.AddItem(f));

            _searchTasks.WaitForComplete();
        }

        private void RecurseDirs(string dir, Action<string> action)
        {
            foreach (var d in Directory.GetDirectories(dir))
                RecurseDirs(d, action);
            foreach (var f in Directory.GetFiles(dir))
            {
                if (IgnoredFiles?.Contains(f) != true)
                    action(f);
            }
        }
    }
}
