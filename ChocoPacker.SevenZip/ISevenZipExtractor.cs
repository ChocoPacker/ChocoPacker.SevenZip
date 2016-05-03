namespace ChocoPacker.SevenZip
{
    public interface ISevenZipExtractor
    {
        IArchive OpenArchive(string path);
    }
}
