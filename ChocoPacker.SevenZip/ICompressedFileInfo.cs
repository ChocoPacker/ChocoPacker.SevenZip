using System;

namespace ChocoPacker.SevenZip
{
    public interface ICompressedFileInfo
    {
        string RelativePath { get; }

        ulong? Size { get; }

        ulong? CompressedSize { get; }

        DateTime? TimeStamp { get; }
    }
}