using System;
using System.IO;

namespace ChocoPacker.SevenZip
{
    internal class TempDirectory : IDisposable
    {
        private readonly string _path;

        private TempDirectory()
        {
            _path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_path);
        }

        public void Dispose() => Directory.Delete(_path, true);

        public override string ToString() => _path;

        public static implicit operator string(TempDirectory temp) => temp._path;

        public static TempDirectory Create() => new TempDirectory();
    }
}
