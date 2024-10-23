using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace DataJob.Server.Helper
{
    public static class LogHelper
    {
        private static ILogger log;
        private static readonly object _lock = new object();

        private static LoggerConfiguration GenLog(this LoggerConfiguration configuration, LogEventLevel logEventLevel, int retainedFileCount = 0, long fileSizeBytes = 5_242_880)
        {
            //日志文件路径
            string LogFilePath(LogEventLevel LogEvent) => $@"{AppContext.BaseDirectory}00_Logs/{LogEvent}/log.log";

            //存储日志文件的格式
            string SerilogOutputTemplate = "{NewLine}{NewLine}Date：{Timestamp:yyyy-MM-dd HH:mm:ss.fff}{NewLine}LogLevel：{Level}{NewLine}Message：{Message}{NewLine}{Exception}" + new string('-', 50);
            configuration.WriteTo.Logger(lg =>
            {
                lg.Filter.ByIncludingOnly(p => p.Level == logEventLevel).WriteTo.File(LogFilePath(logEventLevel),
                    rollingInterval: RollingInterval.Day, outputTemplate: SerilogOutputTemplate, rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: fileSizeBytes, retainedFileCountLimit: retainedFileCount == 0 ? null : retainedFileCount, shared: true);
            });

            return configuration;
        }

        static LogHelper()
        {
            int.TryParse(AppSettingsHelper.GetSetting("LogSetting", "RetainedFileCount"), out int retainedFileCount);
            if (retainedFileCount == 0)
            {
                retainedFileCount = 10;
            }
            long.TryParse(AppSettingsHelper.GetSetting("LogSetting", "FileSizeLimitBytes"), out long fileSizeBytes);

            fileSizeBytes = fileSizeBytes == 0 ? 5 * 1024 * 1024 : fileSizeBytes;
            LoggerConfiguration configuration = new LoggerConfiguration();

            //优化写法
            log = Log.Logger = configuration//new LoggerConfiguration()//.ReadFrom.Configuration(Configuration)
                         .MinimumLevel.Debug()
                         .Enrich.FromLogContext()//使用Serilog.Context.LogContext中的属性丰富日志事件。
                         .GenLog(LogEventLevel.Debug, retainedFileCount, fileSizeBytes)
                         .GenLog(LogEventLevel.Information, retainedFileCount, fileSizeBytes)
                         .GenLog(LogEventLevel.Warning, retainedFileCount, fileSizeBytes)
                         .GenLog(LogEventLevel.Error, retainedFileCount, fileSizeBytes)
                         .GenLog(LogEventLevel.Fatal, retainedFileCount, fileSizeBytes)
                         .CreateLogger();
        }

        #region 日志方法

        public static void Info(string data)
        {
            log.Information("{data}", data);
        }

        public static void Debug(string data)
        {
            log.Debug("{data}", data);
        }

        public static void Error(string data)
        {
            log.Error("{data}", data);
        }

        public static void Warning(string data)
        {
            log.Warning("{data}", data);
        }

        public static void Fatal(string data)
        {
            log.Fatal("{data}", data);
        }

        #endregion

        public static void WriteLog(string fileName, string[] datas, string defaultFolder = "")
        {
            string logContent = string.Join("\r\n", datas);
            WriteLog(fileName, logContent, defaultFolder);
        }


        public static void WriteLog(string fileName, string logContent, string defaultFolder = "")
        {
            int retainedFileCount = 0;

            int.TryParse(AppSettingsHelper.GetSetting("LogSetting", "RetainedFileCount"), out retainedFileCount);
            if (retainedFileCount == 0)
            {
                retainedFileCount = 10;
            }
            long.TryParse(AppSettingsHelper.GetSetting("LogSetting", "FileSizeLimitBytes"), out long fileSizeBytes);
            fileSizeBytes = fileSizeBytes == 0 ? 5 * 1024 * 1024 : fileSizeBytes;
            string logFolder = Path.Combine(AppContext.BaseDirectory, "Log");
            lock (_lock)
            {
                Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(Path.Combine(logFolder, "", @$"{fileName}.log"),// _{DateTime.Now:yyyy-MM-dd}.log"),// MARK 根据日写日志，因为自定义时间格式会影响自动清理逻辑
                              retainedFileCountLimit: retainedFileCount == 0 ? null : retainedFileCount,
                              rollingInterval: RollingInterval.Day,
                              outputTemplate: "{Message}{NewLine}{Exception}", rollOnFileSizeLimit: true, fileSizeLimitBytes: fileSizeBytes, shared: true)
                .CreateLogger();

                Log.Information(DateTime.Now.ToString("HH:mm:ss.ffffff") + ": " + logContent);
                Log.CloseAndFlush();
            }
        }
    }
}
