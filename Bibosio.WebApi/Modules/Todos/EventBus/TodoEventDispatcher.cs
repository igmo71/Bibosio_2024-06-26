using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos.EventBus.Events;
using Bibosio.WebApi.Modules.Todos.Interfaces;

namespace Bibosio.WebApi.Modules.Todos.EventBus
{
    public class TodoEventDispatcher : BackgroundService
    {
        private readonly ITodoEventChannel _eventChannel;
        private readonly ILogger<TodoEventDispatcher> _logger;

        public static event EventHandler<TodoCreatedEvent>? TodoCreated;
        public static event EventHandler<TodoUpdatedEvent>? TodoUpdated;

        public TodoEventDispatcher(ITodoEventChannel eventChannel, ILogger<TodoEventDispatcher> logger)
        {
            _eventChannel = eventChannel;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var integrationEvent in _eventChannel.Reader.ReadAllAsync())
            {
                _logger.LogDebug("{BackgroundService} Read {@IntegrationEvent}", nameof(TodoEventDispatcher), integrationEvent);

                try
                {
                    switch (integrationEvent.GetType().Name)
                    {
                        case nameof(TodoCreatedEvent):
                            await OnTodoCreated(integrationEvent);
                            break;

                        case nameof(TodoUpdatedEvent):
                            await OnTodoUpdated(integrationEvent);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{Message} {@IntegrationEventId}", ex.Message, integrationEvent);
                }
            }

        }

        private async Task OnTodoCreated(IIntegrationEvent integrationEvent)
        {
            await Task.Delay(1000);
            _logger.LogDebug("{Method} {@IntegrationEvent}", nameof(OnTodoCreated), integrationEvent);

            EventHandler<TodoCreatedEvent>? todoCreatedHandler = TodoCreated;
            todoCreatedHandler?.Invoke(this, (TodoCreatedEvent)integrationEvent);
        }

        private async Task OnTodoUpdated(IIntegrationEvent integrationEvent)
        {
            await Task.Delay(1000);
            _logger.LogDebug("{Method} {@IntegrationEvent}", nameof(OnTodoUpdated), integrationEvent);

            EventHandler<TodoUpdatedEvent>? todoUpdatedHandler = TodoUpdated;
            todoUpdatedHandler?.Invoke(todoUpdatedHandler, (TodoUpdatedEvent)integrationEvent);
        }
    }
}
