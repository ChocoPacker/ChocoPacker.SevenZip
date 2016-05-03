using System;
using System.Collections.Generic;
using System.IO;

namespace ChocoPacker.SevenZip
{
    public interface IArchive : IDisposable
    {
        IEnumerable<ICompressedFileInfo> GetArchiveFiles();

        void ExtractFile(ICompressedFileInfo file, string pathToExtract);

        Stream ExtractFile(ICompressedFileInfo file);
    }
}
