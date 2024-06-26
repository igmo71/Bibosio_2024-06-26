using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    internal sealed class IntegrationEventProcessorJob(
    InMemoryMessageQueue queue,
    ILogger<IntegrationEventProcessorJob> logger)
    : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (IIntegrationEvent integrationEvent in
                queue.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await Task.Delay(2000);

                    // TODO: Publish Event

                    logger.LogDebug("IntegrationEventProcessorJob Read {@integrationEvent}", integrationEvent);
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "Something went wrong! {IntegrationEventId}",
                        integrationEvent.EventId);
                }
            }
        }
    }

}
