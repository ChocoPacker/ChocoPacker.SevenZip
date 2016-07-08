using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChocoPacker.SevenZip
{
    internal class Archive : IArchive
    {
        private static readonly Regex SHeaderRegex =
            new Regex(@"Date\s+Time\s+Attr\s+Size\s+Compressed\s+Name",
                RegexOptions.Compiled
                | RegexOptions.CultureInvariant
                | RegexOptions.Singleline);

        private static readonly Regex SFooterRegex =
            new Regex(@"^[-|\s]+$",
                RegexOptions.Compiled
                | RegexOptions.CultureInvariant
                | RegexOptions.Singleline);

        private readonly string _sevenZipPath;

        private readonly string _archivePath;

        /// <summary>
        /// File lock, will prevent other process from file removal until actions with file in progress.
        /// </summary>
        private readonly IDisposable _archiveFile;

        public Archive(string sevenZipPath, string archivePath)
        {
            if (string.IsNullOrEmpty(archivePath))
                throw new ArgumentNullException(nameof(archivePath));

            if (!File.Exists(archivePath))
                throw new ArgumentException($"Invalid path to archive provided, archive doesn't exist: '{archivePath}'");

            _sevenZipPath = sevenZipPath;
            _archivePath = archivePath;
            _archiveFile = File.OpenRead(archivePath);
        }

        public IEnumerable<ICompressedFileInfo> GetArchiveFiles()
            => ParseArchiverOutput(ExecuteSevenZipProcess($"l \"{_archivePath}\" -y"));

        public void ExtractFile(ICompressedFileInfo file, string pathToExtract)
        {
            ValidateCompressedFileInfo(file);
            if (!Path.IsPathRooted(pathToExtract))
                throw new ArgumentException($"Only full paths are supported. Path: '{pathToExtract}' not full!");

            if (File.Exists(pathToExtract))
                throw new ArgumentException($"File already exists {pathToExtract}, won't replace!");

            var directoryName = Path.GetDirectoryName(pathToExtract);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            using (var directory = TempDirectory.Create())
            {
                ExecuteSevenZipProcess($"e \"{_archivePath}\" -y -o\"{directory}\" \"{file.RelativePath}\"");
                var extractedPath = Path.Combine(directory, file.RelativePath);
                File.Copy(extractedPath, pathToExtract);
            }
        }

        public Stream ExtractFile(ICompressedFileInfo file)
        {
            ValidateCompressedFileInfo(file);
            var directory = TempDirectory.Create();
            ExecuteSevenZipProcess($"e \"{_archivePath}\" -y -o\"{directory}\" \"{file.RelativePath}\"");
            return new ArchiveItemStream(directory, Path.Combine(directory, file.RelativePath));
        }

        internal static IEnumerable<ICompressedFileInfo> ParseArchiverOutput(string output)
        {
           var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .SkipWhile(x => !SHeaderRegex.IsMatch(x))
                .Skip(1)
                .ToArray();

            var header = lines.First().Split(' ');
            var dateLength = header[0].Length;
            var attrLength = header[1].Length;
            var sizeLength = header[2].Length;
            var compressedLength = header[3].Length;
            return lines.Skip(1).TakeWhile(x => !SFooterRegex.IsMatch(x))
                .Select(x =>
                {
                    var dateString = x.Substring(0, dateLength);
                    var sizeString = x.Substring(dateLength + attrLength + 3, sizeLength);
                    var compressedString = x.Substring(dateLength + attrLength + sizeLength + 4,
                        compressedLength);
                    var relativePath = x.Substring(dateLength + attrLength + sizeLength + compressedLength + 5)
                        .Trim();
                    DateTime timeStamp;
                    return new CompressedFileInfo
                    {
                        TimeStamp = DateTime.TryParse(dateString, out timeStamp)
                            ? (DateTime?)timeStamp
                            : null,
                        Size = ParseUlong(sizeString),
                        CompressedSize = ParseUlong(compressedString),
                        RelativePath = relativePath
                    };
                });
        }

        private static ulong? ParseUlong(string strToParse)
        {
            ulong value;
            return ulong.TryParse(strToParse.Trim(), out value)
                ? (ulong?)value
                : null;
        }

        private void ValidateCompressedFileInfo(ICompressedFileInfo file)
        {
            if (!(file is CompressedFileInfo))
                throw new ArgumentException($"Basic {nameof(IArchive)} implementation support only internal {nameof(CompressedFileInfo)}.");
        }

        private string ExecuteSevenZipProcess(string arguments)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = _sevenZipPath,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                Arguments = arguments
            };

            using (var process = new Process {StartInfo = processStartInfo})
            {
                var outputBuilder = new StringBuilder();
                process.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new InvalidOperationException("Archive looks broken, can't extract file list!");

                return outputBuilder.ToString();
            }
        }

        public void Dispose() => _archiveFile.Dispose();
    }
}
