using Prism.Logging;
using System;
using System.IO;

namespace PluginNs.Services.Logging
{
    public class Logger : LoggerBase
    {
        public Logger(TextWriter writer)
            : base(writer)
        { }

        protected override void Log(DateTime date, string path, string memberName, int lineNumber, string msg)
        {
            var logMsg = string.Format("{0}:{1}:{2} - {3}", path, memberName, lineNumber, msg);
            Log(logMsg, Category.Info, Priority.None);
        }
    }
}
