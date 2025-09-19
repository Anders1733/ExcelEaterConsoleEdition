using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelEaterConsoleEdition.Utilities
{
    public static class Logger
    {
        public static bool IsLoggingEnabled { get; set; } = true; // Включено по умолчанию
        public static bool IsPerformanceLoggingEnabled { get; set; } = true; // Включено по умолчанию

        private const string LogFormat = "{0}: {1}";

        public static void Info(string message)
        {
            if (!IsLoggingEnabled) return;
            WriteLog("INFO", message);
        }

        public static void Warning(string message)
        {
            if (!IsLoggingEnabled) return;
            WriteLog("WARNING", message);
        }

        public static void Error(string message)
        {
            if (!IsLoggingEnabled) return;
            WriteLog("ERROR", message);
        }

        public static void Performance(string message)
        {
            if (!IsLoggingEnabled || !IsPerformanceLoggingEnabled) return;
            WriteLog("PERF", message);
        }

        private static void WriteLog(string level, string message)
        {
            var formattedMessage = string.Format(LogFormat, DateTime.Now.ToString("HH:mm:ss"), $"[{level}] {message}");
            Console.WriteLine(formattedMessage);
        }
    }
}
