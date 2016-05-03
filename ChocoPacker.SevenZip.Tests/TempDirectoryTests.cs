using System.IO;
using Xunit;

namespace ChocoPacker.SevenZip.Tests
{
    public class TempDirectoryTests
    {
        [Fact]
        public void Create_Create_New_Temp_Directory()
        {
            using (var tempDirectory = TempDirectory.Create())
                Assert.True(Directory.Exists(tempDirectory));
        }

        [Fact]
        public void Create_Dispose_Removes_New_Temp_Directory()
        {
            var tempDirectory = TempDirectory.Create();
            tempDirectory.Dispose();
            Assert.False(Directory.Exists(tempDirectory));
        }
    }
}
