using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    public abstract class EventBus : IEventBus
    {
        protected readonly IEventChannel _eventChannel;
        protected readonly ILogger<IEventBus> _logger;

        public EventBus(IEventChannel eventChannel, ILogger<IEventBus> logger)
        {
            _eventChannel = eventChannel;
            _logger = logger;
        }

        public async Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = default) 
            where TEvent : class, IIntegrationEvent
        {
            await _eventChannel.Writer.WriteAsync(integrationEvent, cancellationToken);
            _logger.LogDebug("{Method} {@IntegrationEvent}", nameof(PublishAsync), integrationEvent);
        }
    }
}
