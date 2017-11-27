using PluginNs.ViewModels;
using System.Windows.Controls;

namespace PluginNs.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    partial class SettingsView : UserControl
    {
        public SettingsView(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
