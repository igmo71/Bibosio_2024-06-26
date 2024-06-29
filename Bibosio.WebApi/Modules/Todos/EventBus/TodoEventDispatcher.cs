using Bibosio.WebApi.Modules.Todos.Interfaces;

namespace Bibosio.WebApi.Modules.Todos.EventBus
{
    public class TodoEventDispatcher : BackgroundService
    {
        private readonly ITodoEventChannel _eventChannel;
        private readonly ILogger<TodoEventDispatcher> _logger;

        public TodoEventDispatcher(ITodoEventChannel eventChannel, ILogger<TodoEventDispatcher> logger)
        {
            _eventChannel = eventChannel;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach(var integrationEvent in _eventChannel.Reader.ReadAllAsync())
            {
                try
                {
                    await Task.Delay(2000);

                    // TODO: Publish Event

                    _logger.LogDebug("{BackgroundService} Read {@IntegrationEvent}", nameof(TodoEventDispatcher), integrationEvent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{Message} {@IntegrationEventId}", ex.Message, integrationEvent);
                }
            }
        }
    }
}
