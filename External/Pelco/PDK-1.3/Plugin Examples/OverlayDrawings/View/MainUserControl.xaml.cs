using System;
using Pelco.Phoenix.PluginHostInterfaces;
using System.Windows.Controls;

namespace OverlayDrawings.View
{
    /// <summary>
    /// Interaction logic for MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        private IOCCHostOverlayDrawingService _drawingService;

        public MainUserControl() : this(null)
        {

        }

        public MainUserControl(IOCCHostOverlayDrawingService drawingService)
        {
            InitializeComponent();
            DataContext = new Model.MainUserControlViewModel(drawingService);

            // Behavior with text boxes changed from DotNet4 to 4.5 and you cannot enter decimal
            // point values when binding to properties unless this value is set.
            try
            {
                System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            _drawingService = drawingService;

            // Register for shutdown start event so we can clean up the animation timer.
            this.Dispatcher.ShutdownStarted += ShutdownStarted;
        }

        public void ShutdownStarted(object sender, EventArgs args)
        {
            (DataContext as Model.MainUserControlViewModel).Dispose();
        }
    }
}
