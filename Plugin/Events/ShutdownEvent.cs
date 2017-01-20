using Prism.Events;

namespace PluginNs.Events
{
    class ShutdownStarted : PubSubEvent<object>
    { }

    class ShutdownCompleted : PubSubEvent<object>
    { }
}
