namespace PluginNs.Services.VxSdk
{
    interface IVxSdkSvc
    {
        /// <summary>
        /// Get the currently logged in VideoXpert user
        /// </summary>
        /// <returns></returns>
        string GetUser();
    }
}
