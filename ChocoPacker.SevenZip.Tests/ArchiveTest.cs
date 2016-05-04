using System.IO;
using System.Linq;
using Xunit;

namespace ChocoPacker.SevenZip.Tests
{
    public class ArchiveTest
    {
        [Fact]
        public void GetArchiveFiles_Return_List_Of_Files()
        {
            using (var archive = CreateArchive())
            {
                var files = archive.GetArchiveFiles().ToArray();
                Assert.True(files.Any());
            }
        }

        [Fact]
        public void ExtractFile_Extracts_File()
        {
            using (var tempPath = TempPath.Create())
            using (var archive = CreateArchive())
            {
                var file = archive.GetArchiveFiles().First(x => x.RelativePath == "0");
                archive.ExtractFile(file, tempPath);
                Assert.True(File.Exists(tempPath));
            }
        }

        [Fact]
        public void ExtractFile_Extracts_Correct_Stream()
        {
            using (var archive = CreateArchive())
            {
                var file = archive.GetArchiveFiles().First(x => x.RelativePath == "0");
                using (var stream = archive.ExtractFile(file))
                {
                    var reader = new StreamReader(stream);
                    var content = reader.ReadToEnd();
                    Assert.True(content.Length > 0);
                    Assert.True(content.StartsWith("<?xml"));
                }
            }
        }

        [Fact]
        public void ParseArchiverOutput_Works_Properly()
        {
            var output = ResourceUtil.ReadResource("ChocoPacker.SevenZip.Tests.TestData.SevenZipFileList.txt");
            var archiverResults = Archive.ParseArchiverOutput(output).ToArray();
            Assert.Equal(7, archiverResults.Length);
            Assert.Equal("0", archiverResults[0].RelativePath);
            Assert.Equal("u0", archiverResults[1].RelativePath);
            Assert.Equal("u1", archiverResults[2].RelativePath);
            Assert.Equal("u2", archiverResults[3].RelativePath);
            Assert.Equal("u3", archiverResults[4].RelativePath);
            Assert.Equal("u4", archiverResults[5].RelativePath);
            Assert.Equal("u5", archiverResults[6].RelativePath);
            Assert.False(archiverResults.Any(x => x.Size == null));
            Assert.Equal(7, archiverResults.Count(x => x.CompressedSize == null));
            Assert.Equal(7, archiverResults.Count(x => x.TimeStamp != null));
        }

        private static IArchive CreateArchive()
            => new SevenZipExtractor(Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "7z.exe"))
                .OpenArchive(Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "dotnet-dev-win-x64.latest.exe"));
    }
}
