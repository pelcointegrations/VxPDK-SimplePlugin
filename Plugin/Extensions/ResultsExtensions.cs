using VxSdkNet;

namespace PluginNs.Extensions
{
    static class ResultsExtensions
    {
        public static bool IsSuccessful(this Results.Value result)
        {
            return result == Results.Value.OK || result == Results.Value.SdkLicenseGracePeriodActive;
        }
    }
}
