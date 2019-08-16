using System;

namespace PluginNs.Services.PluginHost
{
    interface IPluginHostSvc
    {
        /// <summary>
        /// Request that this plugin be closed by OpsCenter
        /// </summary>
        void RequestClose();
        /// <summary>
        /// Store credentials required by this plugin in VideoXpert
        /// </summary>
        /// <param name="credentials"></param>
        void StoreCredentials(string credentials);
        /// <summary>
        /// Set the plugin's width and dock it to the right
        /// </summary>
        /// <param name="pixels"></param>
        void SetPluginWidthDockRight(int pixels);
        /// <summary>
        /// Dock this plugin on the right side of the cell
        /// </summary>
        void DockRight();
        /// <summary>
        /// Dock this plugin in front of the cell
        /// </summary>
        void DockFront();
        /// <summary>
        /// Seek the video in this cell to a particular time
        /// </summary>
        /// <param name="position"></param>
        void SetVideoPosition(DateTime position);
        /// <summary>
        /// Get the auth token from OpsCenter to use in the VxSdk
        /// </summary>
        /// <returns></returns>
        string GetAuthToken();
        /// <summary>
        /// Get the VideoXpert base uri to connect to
        /// </summary>
        /// <returns></returns>
        string GetBaseUri();
    }
}
