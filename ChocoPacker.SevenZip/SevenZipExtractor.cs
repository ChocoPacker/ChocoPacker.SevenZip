using System;
using System.IO;

namespace ChocoPacker.SevenZip
{
    public class SevenZipExtractor : ISevenZipExtractor
    {
        private readonly string _sevenZipPath;

        public SevenZipExtractor(string sevenZipPath)
        {
            if (string.IsNullOrEmpty(sevenZipPath))
                throw new ArgumentNullException(nameof(sevenZipPath));

            if (!File.Exists(sevenZipPath))
                throw new ArgumentException($"Invalid path to 7za.exe provided: '{sevenZipPath}'");

            _sevenZipPath = sevenZipPath;
        }

        public IArchive OpenArchive(string path) => new Archive(_sevenZipPath, path);
    }
}
