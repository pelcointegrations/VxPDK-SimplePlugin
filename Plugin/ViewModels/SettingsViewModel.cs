using PluginNs.Services.VxSdk;
using PluginNs.Utilities;

namespace PluginNs.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        private string _user;

        public SettingsViewModel(IVxSdkSvc serenity)
        {
            var ser = serenity;
            User = ser.GetUser();
        }

        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private void SaveSettings()
        {
            string settingsData = Utils.I.Serialize(Settings);
            PluginHost.StoreCredentials(settingsData);
        }

        protected override void Dispose(bool dispose)
        {
            base.Dispose(dispose);
            SaveSettings();
        }
    }
}
