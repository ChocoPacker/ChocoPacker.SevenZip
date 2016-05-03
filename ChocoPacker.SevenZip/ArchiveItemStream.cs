using System.IO;

namespace ChocoPacker.SevenZip
{
    internal class ArchiveItemStream : Stream
    {
        private readonly TempDirectory _directory;

        private readonly Stream _fileStream; 

        public ArchiveItemStream(TempDirectory directory, string filePath)
        {
            _directory = directory;
            _fileStream = File.OpenRead(filePath);
        }

        public override void Flush() => _fileStream.Flush();

        public override long Seek(long offset, SeekOrigin origin)
            => _fileStream.Seek(offset, origin);

        public override void SetLength(long value)
            => _fileStream.SetLength(value);

        public override int Read(byte[] buffer, int offset, int count)
            => _fileStream.Read(buffer, offset, count);

        public override void Write(byte[] buffer, int offset, int count) 
            => _fileStream.Write(buffer, offset, count);

        public override bool CanRead => _fileStream.CanRead;

        public override bool CanSeek => _fileStream.CanSeek;

        public override bool CanWrite => _fileStream.CanWrite;

        public override long Length => _fileStream.Length;

        public override long Position
        {
            get { return _fileStream.Position; }
            set { _fileStream.Position = value; }
        }

        protected override void Dispose(bool disposing)
        {
            _fileStream.Dispose();
            _directory.Dispose();
            base.Dispose(disposing);
        }
    }
}
