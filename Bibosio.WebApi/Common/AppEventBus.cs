using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    public class AppEventBus : EventBus
    {
        public AppEventBus(IEventChannel eventChannel, ILogger<AppEventBus> logger) : base(eventChannel, logger) { }
    }
}
