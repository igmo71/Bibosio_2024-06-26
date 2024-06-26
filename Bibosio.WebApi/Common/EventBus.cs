using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    internal sealed class EventBus(InMemoryMessageQueue queue, ILogger<EventBus> logger) : IEventBus
    {
        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) 
            where T : class, IIntegrationEvent
        {
            await queue.Writer.WriteAsync(integrationEvent, cancellationToken);
            logger.LogDebug("{Method} {@IntegrationEvent}", nameof(PublishAsync), integrationEvent);
        }
    }

}
