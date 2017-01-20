using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OverlayDrawings.Model.Utils
{
    namespace Delegates
    {
        /// <summary>
        /// Delegate definition for drawing overlays.
        /// </summary>
        /// <param name="overlay">The overlay to draw</param>
        /// <param name="animate">Flag enabling overlay animation</param>
        public delegate void DrawOverlay(Pelco.Phoenix.PluginHostInterfaces.Drawing.Overlay overlay, bool animate);

        /// <summary>
        /// Delegate definition for updating an existing overlay.
        /// </summary>
        /// <param name="overlay">The overlay to update</param>
        public delegate void UpdateOverlay(Pelco.Phoenix.PluginHostInterfaces.Drawing.Overlay overlay);

        /// <summary>
        /// Delegate definition for removing an overlay.
        /// </summary>
        /// <param name="overlay"></param>
        public delegate void RemoveOverlay(Pelco.Phoenix.PluginHostInterfaces.Drawing.Overlay overlay);

        /// <summary>
        /// Delegate definition for removing all overlays
        /// </summary>
        public delegate void ClearOverlays();
    }

    public static class UI
    {
        /// <summary>
        /// Pops up a informational dialog
        /// </summary>
        /// <param name="message"></param>
        public static void ShowInfoDialog(String message)
        {
            ShowDialog(message, "Information", MessageBoxImage.Information);
        }

        /// <summary>
        /// Pops up a warning dialog
        /// </summary>
        /// <param name="message"></param>
        public static void ShowWarningDialog(String message)
        {
            ShowDialog(message, "Warning", MessageBoxImage.Warning);
        }

        /// <summary>
        /// Pops up a error dialog
        /// </summary>
        /// <param name="message"></param>
        public static void ShowErrorDialog(string message)
        {
            ShowDialog(message, "Error", MessageBoxImage.Error);
        }

        private static void ShowDialog(string message, string title, MessageBoxImage image)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, image);
        }
    }
}
