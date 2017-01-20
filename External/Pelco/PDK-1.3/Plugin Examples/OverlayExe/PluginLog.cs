using System;
using System.Diagnostics;


namespace PluginLogger
{
    public class PluginLog
    {
        // Note: TraceSource is thread safe.
        private TraceSource _piTrace;

        public PluginLog(string traceName)
        {
            _piTrace = new TraceSource(traceName);
        }

        public void TraceEvent(TraceEventType traceEventType, int id, String message)
        {
            if (_piTrace == null) return;

            string timeMessage = DateTime.Now.ToString() + ": " + message;
            _piTrace.TraceEvent(traceEventType, id, timeMessage);
            _piTrace.Flush(); // Possible performance improvement, remove this call to Flush() and make sure Close gets called.
        }

        public void Close()
        {
            if (_piTrace == null) return;

            _piTrace.Flush();
            _piTrace.Close();
            _piTrace = null;
        }
    }

}
