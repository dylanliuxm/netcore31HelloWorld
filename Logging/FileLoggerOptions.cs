namespace netcore.Logging
{
    /// <summary>
    /// 文件日志配置（对应 appsettings 中 FileLogging 节）。
    /// </summary>
    public class FileLoggerOptions
    {
        /// <summary>日志目录，相对内容根或绝对路径；默认 logs/</summary>
        public string Path { get; set; } = "logs";

        /// <summary>日志文件名前缀，最终形如 app-yyyy-MM-dd.log</summary>
        public string FileNamePrefix { get; set; } = "app";

        /// <summary>单文件最大字节数，超出后按序号滚动；默认 10MB</summary>
        public long MaxFileSizeBytes { get; set; } = 10 * 1024 * 1024;

        /// <summary>最低写入级别（与 Logging:LogLevel 配合过滤）</summary>
        public string MinLevel { get; set; } = "Information";
    }
}
