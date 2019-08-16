using NLog;
using System;

namespace PluginNs.Extensions
{
    static class LoggerExtensions
    {
        public static void LogThenThrow(this Logger log, Exception e, string message = "")
        {
            log.Error(e, message);
            throw e;
        }
    }
}
