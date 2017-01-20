using System.Diagnostics;
using System.IO;
using System.Text;

namespace PluginNs.Services.Logging
{
    class TraceWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        public override void WriteLine(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
