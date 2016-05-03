using System;
using System.IO;

namespace ChocoPacker.SevenZip.Tests
{
    internal class TempPath : IDisposable
    {
        private readonly string _path;

        private TempPath()
        {
            _path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        public void Dispose()
        {
            if (File.Exists(_path))
                File.Delete(_path);
        }

        public override string ToString() => _path;

        public static implicit operator string(TempPath path) => path._path;

        public static TempPath Create() => new TempPath();
    }
}
