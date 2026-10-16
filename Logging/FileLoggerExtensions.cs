using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace netcore.Logging
{
    /// <summary>
    /// 注册文件日志提供程序的扩展方法。
    /// </summary>
    public static class FileLoggerExtensions
    {
        /// <summary>
        /// 从配置节绑定 FileLoggerOptions 并添加 FileLoggerProvider。
        /// </summary>
        public static ILoggingBuilder AddFile(this ILoggingBuilder builder, IConfiguration section)
        {
            builder.Services.Configure<FileLoggerOptions>(section);
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            return builder;
        }
    }
}
