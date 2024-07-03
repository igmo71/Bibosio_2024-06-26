using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    internal sealed class AppEventDispatcher(IEventChannel channel, ILogger<AppEventDispatcher> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (IIntegrationEvent integrationEvent in channel.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await Task.Delay(1000); // TODO: Example. Remove in prod.

                    // TODO: Publish Event

                    logger.LogDebug("{BackgroundService} Read {@IntegrationEvent}", nameof(AppEventDispatcher), integrationEvent);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{Message} {@IntegrationEventId}", ex.Message, integrationEvent);
                }
            }
        }
    }

}
