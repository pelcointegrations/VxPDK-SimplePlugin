using Pelco.Phoenix.PluginHostInterfaces;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using PluginLogger;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;

namespace DragDropPlugin
{
    class PluginModel : INotifyPropertyChanged
    {
        #region ConstructorAndVariables
        private PluginLog _pluginLog = new PluginLog("TraceEventsMgrPlugin");

        public MainUserControl MainUserControl { get; set; }
        private IHost _host = null;
        private IOCCHost1 _OCCHost1 = null;
        private IOCCHostRegisterForDragDrop _OCCHostDragDrop = null; // DDROP
        private IOCCHostSerenity _OCCHostSerenity = null;
        BackgroundWorker _worker = null;


        // Used for updating listbox on main UI with log contents.
        public ObservableCollection<string> LogEntries { get; set; }
        public Object LockObject = new Object();

        public PluginModel(IHost host)
        {
            // Used for updating listbox on main UI with log contents.
            LogEntries = new ObservableCollection<string>(); // Collection of log entries to be displayed in the example list.
            BindingOperations.EnableCollectionSynchronization(LogEntries, LockObject);

            TraceEvent(TraceEventType.Information, 0, "PluginModel() - Constructor");

            _host = host;
            if (_host != null)
            {
                _OCCHost1 = host.GetService<IOCCHost1>(); 
                TraceEvent(TraceEventType.Information, 0, "PluginModel() -> IHost.GetService<IOCCHost1>()");

                _OCCHostDragDrop = host.GetService<IOCCHostRegisterForDragDrop>(); // DDROP
                TraceEvent(TraceEventType.Information, 0, "PluginModel() -> IHost.GetService<IOCCHostRegisterForDragDrop>()");

                _OCCHostSerenity = host.GetService<IOCCHostSerenity>(); 
                TraceEvent(TraceEventType.Information, 0, "PluginModel() -> IHost.GetService<IOCCHostSerenity>()");

                if (_OCCHost1 == null || _OCCHostSerenity == null || _OCCHostDragDrop == null) // DDROP
                {
                    HostReportFatalError("Unable to get required interfaces.", "IOCCHost1 and/or IOCCHostSerenity and/or IOCCHostRegisterForDragDrop");
                }
            } 
            else
            {
                TraceEvent(TraceEventType.Information, 0, "PluginModel() -> Host interface is null.  We must be running as a stand alone app.");
            }
        }

        private Visibility _loading = Visibility.Visible;
        public Visibility Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                if (_loading == value) return;
                _loading = value;
                NotifyPropertyChanged();
            }
        }

        private Visibility _slidesActive = Visibility.Collapsed; // It is necessary to hide the browser as it doesn't allow the "Loading..." screen to show above it.
        public Visibility EventsPage
        {
            get
            {
                return _slidesActive;
            }
            set
            {
                if (_slidesActive == value) return;
                _slidesActive = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region UtilityMethods

        public void TraceEvent(TraceEventType traceEventType, int id, String message)
        {
            // Log the event to TraceEventsMgrPlugin.txt file next to the .exe file IF the appropriate .config settings are set.
            _pluginLog.TraceEvent(traceEventType, id, message);

            // Add this log entry into the LogEntries list to show whats going on in the UI
            LogEntries.Add(message);
        }

        private void ShowMessage(string message)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                MessageBox.Show(message);
            }));
        }

        private bool CheckHostInterface(bool hostIsNull, string traceTxt)
        {
            if (hostIsNull)
            {
                TraceEvent(TraceEventType.Information, 0, "FAIL: Host interface not available: Attempting " + traceTxt);
                return false;
            }
            else
            {
                TraceEvent(TraceEventType.Information, 0, "Plug-in calling " + traceTxt);
                return true;
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region IHost
        public void HostReportFatalError(string userMessage, string fullExceptionText)
        {
            if (CheckHostInterface((_host==null), "HostReportFatalError()"))
            {
                TraceEvent(TraceEventType.Critical, 0, "UserMessage: " + userMessage + "\nFull Exception Text: " + fullExceptionText);
                _host.ReportFatalError(userMessage, fullExceptionText);
            }
        }

        public void HostRequestClose()
        {
            if (CheckHostInterface((_host == null), "HostRequestClose()"))
            {
                _host.RequestClose();
            }
        }

        public void HostRequestFullScreen()
        {
            if (CheckHostInterface((_host == null), "HostRequestFullScreen()"))
            {
                _host.RequestFullScreen();
            }
        }

        public void HostReturnFromFullScreen()
        {
            if (CheckHostInterface((_host == null), "HostReturnFromFullScreen()"))
            {
                _host.ReturnFromFullScreen();
            }
        }
        #endregion

        #region IOCCHostDragDrop // DDROP

        public void HostDragDropRegisterForDragDrop(bool sendDragDrop)
        {
            if (CheckHostInterface((_OCCHostDragDrop == null), "HostDragDropRegisterForDragDrop()"))
            {
                _OCCHostDragDrop.RegisterForDragDrop(sendDragDrop);
            }
        }

        #endregion

        #region IOCCHost1
        public void Host1RegisterForThumbnailPreference(bool sendThumbnailPreference)
        {
            if (CheckHostInterface((_OCCHost1 == null), "Host1RegisterForThumbnailPreference()"))
            {
                _OCCHost1.RegisterForThumbnailPreference(sendThumbnailPreference);
            }
        }

        public void Host1RegisterForOnScreenNotifications(bool sendOnScreenNotifications)
        {
            if (CheckHostInterface((_OCCHost1 == null), "Host1RegisterForOnScreenNotifications()"))
            {
                _OCCHost1.RegisterForOnScreenNotifications(sendOnScreenNotifications);
            }
        }

        public void Host1InitiateStreams(System.Collections.Generic.List<string> cameraList)
        {
            if (CheckHostInterface((_OCCHost1 == null), "Host1InitiateStreams()"))
            {
                _OCCHost1.InitiateStreams(cameraList);
            }
        }

        public void Host1StoreCredentials(string credentials)
        {
            if (CheckHostInterface((_OCCHost1 == null), "Host1StoreCredentials()"))
            {
                _OCCHost1.StoreCredentials(credentials);
            }
        }
        #endregion

        #region IOCCHostSerenity
        public string HostSerenityGetAuthToken()
        {
            if (CheckHostInterface((_OCCHostSerenity == null), "HostSerenityGetAuthToken()"))
            {
                return _OCCHostSerenity.GetAuthToken();
            }
            return null;
        }

        public string HostSerenityGetBaseURI()
        {
            if (CheckHostInterface((_OCCHostSerenity == null), "HostSerenityGetBaseURI()"))
            {
                return _OCCHostSerenity.GetBaseURI();
            }
            return null;
        }

        public string GetAdminPortalIP()
        {
            string baseURI = HostSerenityGetBaseURI();
            string authToken = HostSerenityGetAuthToken();
            if (string.IsNullOrWhiteSpace(baseURI)) return null;

            string ip = null;
            var index = baseURI.IndexOf("https://");
            if (index == -1)
            {
                index = baseURI.IndexOf("http://");
                if (index == -1)
                {
                    return null;
                } 
                else
                {
                    ip = baseURI.Remove(0, 7);
                }
            } 
            else
            {
                ip = baseURI.Remove(0, 8);

            }

            if (ip.Contains(":"))
            {
                ip = ip.Remove(ip.IndexOf(':'));
            } 
            else if (ip.Contains("/"))
            {
                ip = ip.Remove(ip.IndexOf('/'));
            }

            ip = ip + "/portal/System.html?auth_token=" + authToken.TrimEnd('=');
            // TraceEvent(TraceEventType.Information, 0, "Admin Portal Address: " + ip);

            return ip;
        }
        #endregion

        #region IPlugin
        public FrameworkElement PluginCreateControl()
        {
            // Note: DO NOT DO BLOCK THIS API FROM RETURNING. IT WILL MAKE THE OPS CENTER APPEAR TO HANG.
            // Message boxes, synchronous calls to a server, bad ideas here.  Create the framework element and return.
            if (MainUserControl == null)
            {
                MainUserControl = new MainUserControl();
                MainUserControl.DataContext = this;
                MainUserControl.Loaded += MainUserControl_Loaded;
            }

            return MainUserControl;
        }

        #region LoadingMethods
        public void MainUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Note: DO NOT DO BLOCK THIS API FROM RETURNING. IT WILL MAKE THE OPS CENTER APPEAR TO HANG.
            // Message boxes, synchronous calls to a server, bad ideas here.  Spin up a new thread and do your work asynchronously.
            // Return from this asap.
            TraceEvent(TraceEventType.Information, 0, "MainUserControl_Loaded() -> Start a BackgroundWorker() to complete loading.");
            _worker = new BackgroundWorker();
            _worker.DoWork += new DoWorkEventHandler(worker_LoadInBackground);
            _worker.RunWorkerAsync();
        }

        void worker_LoadInBackground(object sender, DoWorkEventArgs e)
        {
            // Do background loading stuff here.
            Thread.Sleep(2000);
            HostDragDropRegisterForDragDrop(true); // DDROP
            // When you are done, hide the loading UI.

            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                TraceEvent(TraceEventType.Information, 0, "worker_LoadInBackground() -> Finished loading in the background.");
                Loading = Visibility.Hidden;
                EventsPage = Visibility.Visible;
            }));
        }

        #endregion

        public void PluginShutdown()
        {
            TraceEvent(TraceEventType.Information, 0, "PluginShutdown()");
            HostRequestClose();  // This is the last thing we do.  The Ops Center will unload us after we RequestClose().
        }
        #endregion

        #region IOCCPlugin1
        public string WebAddress { get; set; }

        public string Plugin1GetPluginState()
        {
            return WebAddress;
        }

        public void Plugin1SetPluginState(string state)
        {
            WebAddress = state;
        }
        #endregion

    }
}
