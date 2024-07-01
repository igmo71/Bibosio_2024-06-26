using Bibosio.WebApi.Modules.Todos.EventBus.Events;
using Bibosio.WebApi.Modules.Todos.Interfaces;

namespace Bibosio.WebApi.Modules.Todos.EventBus
{
    public class TodoEventDispatcher : BackgroundService
    {
        public static event EventHandler<TodoCreatedEvent>? TodoCreated;
        public static event EventHandler<TodoUpdatedEvent>? TodoUpdated;

        private readonly ITodoEventChannel _eventChannel;
        private readonly ILogger<TodoEventDispatcher> _logger;

        public TodoEventDispatcher(
            ITodoEventChannel eventChannel, 
            ILogger<TodoEventDispatcher> logger
            )
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
                    switch (integrationEvent)
                    {
                        case TodoCreatedEvent todoCreatedEvent:
                            await OnTodoCreated(todoCreatedEvent);
                            break;

                        case TodoUpdatedEvent todoUpdatedEvent:
                            await OnTodoUpdated(todoUpdatedEvent);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{Message} {@IntegrationEventId}", ex.Message, integrationEvent);
                }
            }
        }

        private async Task OnTodoCreated(TodoCreatedEvent todoCreatedEvent)
        {
            await Task.Delay(1000); // TODO: Example. Remove in prod.
            _logger.LogDebug("{Method} {@TodoCreatedEvent}", nameof(OnTodoCreated), todoCreatedEvent);

            EventHandler<TodoCreatedEvent>? todoCreatedHandler = TodoCreated;
            todoCreatedHandler?.Invoke(this, todoCreatedEvent);
        }

        private async Task OnTodoUpdated(TodoUpdatedEvent todoUpdatedEvent)
        {
            await Task.Delay(1000); // TODO: Example. Remove in prod.
            _logger.LogDebug("{Method} {@TodoUpdatedEvent}", nameof(OnTodoUpdated), todoUpdatedEvent);

            EventHandler<TodoUpdatedEvent>? todoUpdatedHandler = TodoUpdated;
            todoUpdatedHandler?.Invoke(todoUpdatedHandler, todoUpdatedEvent);
        }
    }
}
