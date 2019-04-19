using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FileMonitor
{
    public static class Logger
    {
        public const int LogTimeout = 2000;
        public const string LogFileName = "FileMonitor.log";

        private static Task logTask;
        private static ConcurrentQueue<string> logQueue;

        public static string Log(string message)
        {
            logQueue.Enqueue(message);
            return message;
        }

        private static void Processing()
        {
            var logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Log");
            if (!Directory.Exists(logFilePath))
                Directory.CreateDirectory(logFilePath);

            var logFileName = Path.Combine(logFilePath, LogFileName);
            using (var logFile = new FileStream(logFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var writer = new StreamWriter(logFile) { AutoFlush = true })
                {
                    while (true)
                    {
                        while (logQueue.TryDequeue(out var logMessage))
                        {
                            writer.WriteLine(logMessage);
                        }

                        Thread.Sleep(LogTimeout);
                    }
                }
            }
        }

        static Logger()
        {
            logQueue = new ConcurrentQueue<string>();
            logTask = Task.Factory.StartNew(Processing);
        }
    }
}
