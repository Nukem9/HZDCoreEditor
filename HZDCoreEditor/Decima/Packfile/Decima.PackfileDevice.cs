using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Decima
{
    public class PackfileDevice : IDisposable
    {
        public const string AppDirMountPrefix = "appdir:";
        public const string CacheMountPrefix = "cache:";
        public const string SourceMountPrefix = "source:";
        public const string WorkMountPrefix = "work:";

        public IEnumerable<string> ActiveArchives => _mountedArchives.Select(x => x.ArchiveName);
        public IReadOnlyCollection<ulong> ActiveFiles => _corePathToArchiveIndex.Keys;

        private List<MountEntry> _mountedArchives = new List<MountEntry>();
        private Dictionary<ulong, int> _corePathToArchiveIndex = new Dictionary<ulong, int>();

        private class MountEntry
        {
            public string PhysicalPath;
            public string ArchiveName;
            public PackfileReader Archive;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PackfileDevice()
        {
        }

        /// <summary>
        /// Load an archive so it can be read and used. Returns false when the file doesn't exist
        /// or when it's a debug archive.
        /// </summary>
        public bool Mount(string archivePath)
        {
            string physPath = Path.GetFullPath(archivePath);
            string filePart = Path.GetFileName(physPath).ToLower();

            // HZD skips anything with "debug" in the file name
            if (filePart.Contains("debug"))
                return false;

            if (!File.Exists(physPath))
                return false;

            // Sort by name. Patch files must be last as they overwrite everything.
            int entryIndex = 0;
            for (; entryIndex < _mountedArchives.Count; entryIndex++)
            {
                var entry = _mountedArchives[entryIndex];

                if (filePart.Contains("patch") && !entry.ArchiveName.Contains("patch"))
                    continue;

                if (!filePart.Contains("patch") && entry.ArchiveName.Contains("patch"))
                    break;

                int compare = filePart.CompareTo(entry.ArchiveName);

                // Don't allow duplicate archives to be loaded
                if (compare == 0)
                    throw new ArgumentException($"Attempting to load an archive that's already loaded: {filePart}", nameof(archivePath));

                if (compare < 0)
                    break;
            }

            _mountedArchives.Insert(entryIndex, new MountEntry()
            {
                PhysicalPath = physPath,
                ArchiveName = filePart,
                Archive = new PackfileReader(physPath),
            });

            RebuildCorePathLookupTable();
            return true;
        }

        /// <summary>
        /// Load the default archives from a typical install of Horizon Zero Dawn.
        /// </summary>
        public void MountDefaultArchives(string baseDirectoryPath)
        {
            // TODO: Needs to be game specific
            var defaultLoadPriority = new Dictionary<string, int>()
            {
                { "dlc1.bin", 0 },
                { "dlc1_arabic.bin", 1 },
                { "dlc1_english.bin", 2 },
                { "dlc1_french.bin", 3 },
                { "dlc1_german.bin", 4 },
                { "dlc1_italian.bin", 5 },
                { "dlc1_latampor.bin", 6 },
                { "dlc1_latamsp.bin", 7 },
                { "dlc1_polish.bin", 8 },
                { "dlc1_portugese.bin", 9 },
                { "dlc1_russian.bin", 10 },
                { "dlc1_spanish.bin", 11 },
                { "fgrwin32.bin", 12 },
                { "initial.bin", 13 },
                { "initial_arabic.bin", 14 },
                { "initial_english.bin", 15 },
                { "initial_french.bin", 16 },
                { "initial_german.bin", 17 },
                { "initial_italian.bin", 18 },
                { "initial_latampor.bin", 19 },
                { "initial_latamsp.bin", 21 },
                { "initial_polish.bin", 21 },
                { "initial_portugese.bin", 22 },
                { "initial_russian.bin", 23 },
                { "initial_spanish.bin", 24 },
                { "remainder.bin", 25 },
                { "remainder_arabic.bin", 26 },
                { "remainder_english.bin", 27 },
                { "remainder_french.bin", 28 },
                { "remainder_german.bin", 29 },
                { "remainder_italian.bin", 30 },
                { "remainder_latampor.bin", 31 },
                { "remainder_latamsp.bin", 32 },
                { "remainder_polish.bin", 33 },
                { "remainder_portugese.bin", 34 },
                { "remainder_russian.bin", 35 },
                { "remainder_spanish.bin", 36 },
                { "remainder_swedish.bin", 37 },
                { "patch.bin", int.MaxValue },
            };

            foreach (string key in defaultLoadPriority.Keys)
            {
                if (!Mount(Path.Combine(baseDirectoryPath, key)))
                    throw new Exception($"Failed to mount required archive {key}");
            }
        }

        /// <summary>
        /// Unload an archive.
        /// </summary>
        public void Unmount(string archiveName)
        {
            var entry = _mountedArchives.SingleOrDefault(x => x.ArchiveName.Equals(archiveName.ToLower()));

            if (entry == null)
                throw new ArgumentException($"Trying to unmount an archive that's not loaded: {archiveName}", nameof(archiveName));

            _mountedArchives.Remove(entry);
            RebuildCorePathLookupTable();
        }

        /// <summary>
        /// Checks if a Decima-formatted path is present in the mounted archives.
        /// </summary>
        public bool HasFile(string corePath)
        {
            return _corePathToArchiveIndex.ContainsKey(Packfile.GetHashForPath(corePath));
        }

        /// <summary>
        /// Checks if a hashed path is present in the mounted archives.
        /// </summary>
        public bool HasFile(ulong pathId)
        {
            return _corePathToArchiveIndex.ContainsKey(pathId);
        }

        /// <summary>
        /// Forwarder for <see cref="PackfileReader.ExtractFile(string, string, bool)"/>
        /// </summary>
        public void ExtractFile(string corePath, string destinationPath, bool allowOverwrite = false)
        {
            ExtractFile(Packfile.GetHashForPath(corePath), destinationPath, allowOverwrite);
        }

        /// <summary>
        /// Forwarder for <see cref="PackfileReader.ExtractFile(ulong, string, bool)"/>
        /// </summary>
        public void ExtractFile(ulong pathId, string destinationPath, bool allowOverwrite = false)
        {
            using var fs = File.Open(destinationPath, allowOverwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write);
            ExtractFile(pathId, fs);
        }

        /// <summary>
        /// Forwarder for <see cref="PackfileReader.ExtractFile(string, Stream)"/>
        /// </summary>
        public void ExtractFile(string corePath, Stream stream)
        {
            ExtractFile(Packfile.GetHashForPath(corePath), stream);
        }

        /// <summary>
        /// Forwarder for <see cref="PackfileReader.ExtractFile(ulong, Stream)"/>
        /// </summary>
        public void ExtractFile(ulong pathId, Stream stream)
        {
            if (!_corePathToArchiveIndex.TryGetValue(pathId, out int order))
                throw new FileNotFoundException($"Unable to find core path ID {pathId} in mounted archives");

            _mountedArchives[order].Archive.ExtractFile(pathId, stream);
        }

        /// <summary>
        /// Convert a path ID to its source string if the mapping is available.
        /// </summary>
        public string PathIdToFileName(ulong pathId)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            //foreach (var entry in _mountedArchives)
            //    entry.Archive.Dispose();

            _mountedArchives.Clear();
        }

        private void RebuildCorePathLookupTable()
        {
            _corePathToArchiveIndex.Clear();

            // Add files, starting from the highest priority
            // TODO: ConcurrentDictionary + memoize on access insteaad? Needs profiling.
            for (int i = _mountedArchives.Count - 1; i >= 0; i--)
            {
                foreach (var fileEntry in _mountedArchives[i].Archive.FileEntries)
                    _corePathToArchiveIndex.TryAdd(fileEntry.PathHash, i);
            }
        }
    }
}
