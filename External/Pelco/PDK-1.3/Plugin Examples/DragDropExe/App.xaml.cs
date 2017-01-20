using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using PluginLogger;

namespace DragDropPlugin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static PluginLog _traceApp = new PluginLog("TraceEventsMgrApp");

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _traceApp.TraceEvent(TraceEventType.Information, 0, "Application_Startup()");
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _traceApp.TraceEvent(TraceEventType.Information, 0, "Application_Exit()");
            _traceApp.Close();
        }

    }
}
