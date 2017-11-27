using PluginNs.Services.Serenity;
using PluginNs.Utilities;

namespace PluginNs.ViewModels
{
    class SettingsViewModel : BaseViewModel
    {
        private string _user;

        public SettingsViewModel(ISerenity serenity)
        {
            var ser = serenity;
            User = ser.GetUser();
        }

        public string User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private void SaveSettings()
        {
            string settingsData = Utils.Instance.Serialize(Settings);
            PluginHost.StoreCredentials(settingsData);
        }

        protected override void Dispose(bool dispose)
        {
            base.Dispose(dispose);
            SaveSettings();
        }
    }
}
