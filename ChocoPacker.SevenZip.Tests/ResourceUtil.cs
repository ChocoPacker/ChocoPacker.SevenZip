using System.IO;
using System.Reflection;

namespace ChocoPacker.SevenZip.Tests
{
    internal static class ResourceUtil
    {
        public static string ReadResource(string resourcePath)
        {
            var assembly = typeof(ResourceUtil).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(resourcePath);
            using (stream)
            {
                var streamReader = new StreamReader(stream);
                return streamReader.ReadToEnd();
            }
        }
    }
}
