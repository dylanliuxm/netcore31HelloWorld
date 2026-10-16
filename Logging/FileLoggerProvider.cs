using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace netcore.Logging
{
    /// <summary>
    /// 文件日志提供程序：按日期命名，超大小时序号滚动；线程安全追加写入。
    /// </summary>
    [ProviderAlias("File")]
    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private readonly object _writeLock = new object();
        private readonly string _directory;
        private readonly string _fileNamePrefix;
        private readonly long _maxFileSizeBytes;
        private bool _disposed;

        /// <summary>本提供程序接受的最低日志级别</summary>
        public LogLevel MinLevel { get; }

        public FileLoggerProvider(IOptions<FileLoggerOptions> options)
        {
            var opts = options?.Value ?? new FileLoggerOptions();
            _directory = string.IsNullOrWhiteSpace(opts.Path) ? "logs" : opts.Path.Trim();
            _fileNamePrefix = string.IsNullOrWhiteSpace(opts.FileNamePrefix) ? "app" : opts.FileNamePrefix.Trim();
            _maxFileSizeBytes = opts.MaxFileSizeBytes > 0 ? opts.MaxFileSizeBytes : 10 * 1024 * 1024;
            MinLevel = ParseLevel(opts.MinLevel);
        }

        public ILogger CreateLogger(string categoryName) => new FileLogger(categoryName, this);

        /// <summary>
        /// 线程安全地将一行写入当前滚动目标文件。
        /// </summary>
        internal void WriteLine(string line)
        {
            if (_disposed)
            {
                return;
            }

            lock (_writeLock)
            {
                Directory.CreateDirectory(_directory);
                var path = ResolveLogFilePath();
                File.AppendAllText(path, line + Environment.NewLine);
            }
        }

        /// <summary>
        /// 解析当日日志路径；若当前文件已超大小则使用 .1、.2 … 序号后缀。
        /// </summary>
        private string ResolveLogFilePath()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var baseName = $"{_fileNamePrefix}-{date}.log";
            var candidate = Path.Combine(_directory, baseName);

            if (!File.Exists(candidate) || new FileInfo(candidate).Length < _maxFileSizeBytes)
            {
                return candidate;
            }

            var index = 1;
            while (true)
            {
                var rolled = Path.Combine(_directory, $"{_fileNamePrefix}-{date}.{index}.log");
                if (!File.Exists(rolled) || new FileInfo(rolled).Length < _maxFileSizeBytes)
                {
                    return rolled;
                }
                index++;
            }
        }

        private static LogLevel ParseLevel(string value)
        {
            if (Enum.TryParse(value, ignoreCase: true, out LogLevel level))
            {
                return level;
            }
            return LogLevel.Information;
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
