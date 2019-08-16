using NLog;
using PluginNs.Extensions;
using PluginNs.Services.PluginHost;
using PluginNs.Utilities;
using System;
using VxSdkNet;

namespace PluginNs.Services.VxSdk
{
    class VxSdkSvc : IVxSdkSvc
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private IPluginHostSvc _host;
        private VXSystem _system;

        public VxSdkSvc(IPluginHostSvc host)
        {
            _host = host;
            Init();
        }

        private void Init()
        {
            var retVal = VxGlobal.SetLogLevel(Const.VxSdkLogLevel);
            if (retVal != Results.Value.OK)
                Log.LogThenThrow(new Exception($"Unable to set VxSdk log level. {retVal}"));
            retVal = VxGlobal.SetLogPath(Const.VxSdkLogFilePath);
            if (retVal != Results.Value.OK)
                Log.LogThenThrow(new Exception($"Unable to set VxSdk log path. {retVal}"));

            Uri baseUri = new Uri(_host.GetBaseUri());
            string authKey = _host.GetAuthToken();
            _system = new VXSystem(baseUri.Host, Const.SdkLicense);
            retVal = _system.Login(authKey);
            if (!retVal.IsSuccessful())
                Log.LogThenThrow(new Exception($"Unable to login via VxSdk. {retVal}"));
            Log.Debug("Successfully initialized VxSdk");
        }

        private bool EnsureAuthKey()
        {
            var retVal = _system.Refresh();
            if (retVal != Results.Value.OK)
            {
                Uri baseUri = new Uri(_host.GetBaseUri());
                string authKey = _host.GetAuthToken();
                _system.Dispose();

                _system = new VXSystem(baseUri.Host, Const.SdkLicense);
                retVal = _system.Login(authKey);
                if (!retVal.IsSuccessful())
                    Log.Warn($"Unable to refresh auth key/login via VxSdk. {retVal}");
            }
            return retVal.IsSuccessful();
        }

        public string GetUser()
        {
            if (!EnsureAuthKey()) return string.Empty;

            return _system.Currentuser.Name;
        }
    }
}
