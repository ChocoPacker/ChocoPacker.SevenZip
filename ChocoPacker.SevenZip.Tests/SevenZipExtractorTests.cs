using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.IO;
using Xunit;

namespace ChocoPacker.SevenZip.Tests
{
    public class SevenZipExtractorTests
    {
        [Fact]
        public void OpenArchive_Succeed()
        {
            var extractor = new SevenZipExtractor(Path.Combine(ApplicationEnvironment.ApplicationBasePath, "TestFiles", "7z.exe"));
            var archive = extractor.OpenArchive(
                Path.Combine(ApplicationEnvironment.ApplicationBasePath, 
                    "TestFiles", 
                    "dotnet-dev-win-x64.latest.exe"));
            Assert.NotNull(archive);
        }
        
        [Fact]
        public void OpenArchive_Throws_ArgumentExeption_When_File_Not_Exist()
        {
            Assert.Throws<ArgumentException>(() => new SevenZipExtractor("7z.exe"));
        }
    }
}
