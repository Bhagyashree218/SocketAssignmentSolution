using System;
using System.IO;

namespace ServerApp.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly string logFilePath = "logs.txt";
        private readonly object _lock = new object();

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }  

        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        private void WriteLog(string level, string message)
        {
            string logMessage = $"{level}: {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n";

            lock (_lock) 
            {
                File.AppendAllText(logFilePath, logMessage);
            }
        }
    }
}