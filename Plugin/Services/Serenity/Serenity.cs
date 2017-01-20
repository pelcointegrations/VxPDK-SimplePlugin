using CPPCli;
using PluginNs.Services.Host;
using PluginNs.Services.Logging;
using PluginNs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginNs.Services.Serenity
{
    class Serenity : ISerenity
    {
        private ILogger _logger;
        private IPluginHost _host;
        private VXSystem _system;

        public Serenity(ILogger logger, IPluginHost host)
        {
            _logger = logger;
            _host = host;

            Init();
        }

        private void Init()
        {
            var retVal = VxGlobal.InitializeSdk(Const.VxSdkKey);
            if (retVal != Results.Value.OK)
                _logger.LogThenThrow(new Exception($"Unable to init the VxSdk. {retVal}"));
            retVal = VxGlobal.SetLogLevel(Const.VxSdkLogLevel);
            if (retVal != Results.Value.OK)
                _logger.LogThenThrow(new Exception($"Unable to set VxSdk log level. {retVal}"));
            retVal = VxGlobal.SetLogPath(Const.VxSdkLogFilePath);
            if (retVal != Results.Value.OK)
                _logger.LogThenThrow(new Exception($"Unable to set VxSdk log path. {retVal}"));

            Uri baseUri = new Uri(_host.GetBaseUri());
            string authKey = _host.GetAuthToken();
            _system = new VXSystem(baseUri.Host);
            retVal = _system.Login(authKey);
            if (retVal != Results.Value.OK)
                _logger.LogThenThrow(new Exception($"Unable to login via VxSdk. {retVal}"));
            _logger.Log("Successfully initialized VxSdk");
        }

        private bool EnsureAuthKey()
        {
            var retVal = _system.Refresh();
            if (retVal != Results.Value.OK)
            {
                string baseUri = _host.GetBaseUri();
                string authKey = _host.GetAuthToken();
                _system.Dispose();
                _system = new VXSystem(baseUri);
                retVal = _system.Login(authKey);
                if (retVal != Results.Value.OK)
                    _logger.Log($"Unable to refresh auth key/login via VxSdk. {retVal}");
            }
            return retVal == Results.Value.OK;
        }

        public string GetUser()
        {
            if (!EnsureAuthKey()) return string.Empty;

            return _system.Currentuser.Name;
        }
    }
}
