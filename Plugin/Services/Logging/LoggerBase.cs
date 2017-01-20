using Prism.Logging;
using System;
using System.IO;
using System.Text;

namespace PluginNs.Services.Logging
{
    public abstract class LoggerBase : TextLogger, ILogger
    {
        private static readonly object _logLock = new object();

        abstract protected void Log(DateTime date, string path, string memberName, int lineNumber, string msg);

        public LoggerBase(TextWriter writer)
            : base(writer)
        { }

        public void Log(string msg, string path = "", string memberName = "", int lineNumber = 0)
        {
            lock (_logLock)
            {
                Log(DateTime.Now, Path.GetFileNameWithoutExtension(path), memberName, lineNumber, msg);
            }
        }

        public void Log(object msg, string path = "", string memberName = "", int lineNumber = 0)
        {
            if (msg != null)
            {
                var msgString = msg.ToString();
                if (!string.IsNullOrWhiteSpace(msgString))
                    Log(msgString, path, memberName, lineNumber);
            }
        }

        public void Log(string msg, Exception e, string path = "", string memberName = "", int lineNumber = 0)
        {
            lock (_logLock)
            {
                Log(msg, path, memberName, lineNumber);
                Log(e, path, memberName, lineNumber);
            }
        }

        public void Log(Exception e, string path = "", string memberName = "", int lineNumber = 0)
        {
            if (e == null)
                return;

            var sb = new StringBuilder();
            sb.AppendLine(e.GetType().ToString());
            if (!string.IsNullOrWhiteSpace(e.Message))
                sb.AppendLine(e.Message);
            if (!string.IsNullOrWhiteSpace(e.StackTrace))
                sb.AppendLine(e.StackTrace);

            Exception ex = e;
            while (ex != null && ex.InnerException != null && !string.IsNullOrWhiteSpace(e.InnerException.Message))
            {
                sb.AppendLine(e.InnerException.Message);
                ex = ex.InnerException;
            }

            Log(sb.ToString(), path, memberName, lineNumber);
        }

        public void LogThenThrow(Exception e, string path = "", string memberName = "", int lineNumber = 0)
        {
            Log(e, path, memberName, lineNumber);
            throw e;
        }
    }
}
