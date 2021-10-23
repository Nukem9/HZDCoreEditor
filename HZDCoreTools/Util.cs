using Decima;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HZDCoreTools
{
    public static class Util
    {
        public static IEnumerable<(string Absolute, string Relative)> GatherFiles(string inputPath, string[] acceptedExtensions, out string extension)
        {
            // If no directory is supplied, use the current working dir
            string basePath = Path.GetDirectoryName(inputPath);
            string filePart = Path.GetFileName(inputPath);

            if (string.IsNullOrEmpty(basePath))
                basePath = @".\";

            extension = acceptedExtensions.SingleOrDefault(x => filePart.EndsWith(x));

            if (extension == null)
                throw new ArgumentException($"Invalid path supplied. Supported file extension(s): {string.Join(',', acceptedExtensions)}", nameof(inputPath));

            return Directory.EnumerateFiles(basePath, filePart, SearchOption.AllDirectories)
                .Select(x => (x, x.Substring(basePath.Length)));
        }

        public static string RemoveMountPrefixes(string path)
        {
            foreach (string p in PackfileDevice.ValidMountPrefixes.Where(x => path.StartsWith(x, StringComparison.InvariantCultureIgnoreCase)))
                return path.Substring(p.Length);

            return path;
        }
    }
}
