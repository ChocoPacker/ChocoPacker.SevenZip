# 7z.exe wrapper for ChocoPacker

### Build status
Windows x86/x64

[![Build status](https://ci.appveyor.com/api/projects/status/vheo9e6l200d47yc?svg=true)](https://ci.appveyor.com/project/nitridan/nodetocoreclr-iueo1)

Usage example:

```csharp
var extractor = new SevenZipExtractor(@"C:\7zip\7za.exe");           
var archive = extractor.OpenArchive(@"C:\myarchive.zip");
using (archive)
{
   var file = archive.GetArchiveFiles().First(x => x.RelativePath == "file1.xml");
   archive.ExtractFile(file, @"C:\temp\myfile.xml"); 
}
```

Usage exmple with streams:

```csharp
var extractor = new SevenZipExtractor(@"C:\7zip\7za.exe");           
var archive = extractor.OpenArchive(@"C:\myarchive.zip");
using (archive)
{
   var file = archive.GetArchiveFiles().First(x => x.RelativePath == "file1.xml");
   archive.ExtractFile(file, @"C:\temp\myfile.xml");
   using (var stream = archive.ExtractFile(file))
   {
       var reader = new StreamReader(stream);
       var content = reader.ReadToEnd();
       Console.WriteLine(content);
   }
}
```