using System;

namespace ChocoPacker.SevenZip
{
    internal class CompressedFileInfo : ICompressedFileInfo
    {
        public string RelativePath { get; set; }

        public ulong? Size { get; set; }

        public ulong? CompressedSize { get; set; }

        public DateTime? TimeStamp { get; set; }
    }
}