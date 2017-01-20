using Prism.Logging;
using System;
using System.Runtime.CompilerServices;

namespace PluginNs.Services.Logging
{
    public interface ILogger : ILoggerFacade, IDisposable
    {
        void Log(string msg, [CallerFilePath] string path = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);
        void Log(object msg, [CallerFilePath] string path = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);
        void Log(string msg, Exception e, string path = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);
        void Log(Exception e, [CallerFilePath] string path = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);
        void LogThenThrow(Exception e, [CallerFilePath] string path = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0);
    }
}
