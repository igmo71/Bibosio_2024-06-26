using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos.EventBus;
using Bibosio.WebApi.Modules.Todos.EventBus.Events;

namespace Bibosio.WebApi.Modules.Todos.EventHandlers.Handlers
{
    public class TodoUpdatedEventHandler : IEventHandler<TodoUpdatedEvent>, IDisposable
    {
        private readonly ILogger<TodoUpdatedEventHandler> _logger;

        public TodoUpdatedEventHandler(ILogger<TodoUpdatedEventHandler> logger)
        {
            TodoEventDispatcher.TodoUpdated += async (sender, integrationEvent) => await HandleAsync(integrationEvent);
            _logger = logger;
        }

        public async Task HandleAsync(TodoUpdatedEvent integrationEvent)
        {
            await Task.Delay(1000); // TODO: Remove in prod
            _logger.LogDebug("{Object} {Method} {@TodoUpdatedEvent}", nameof(TodoUpdatedEventHandler), nameof(HandleAsync), integrationEvent);
        }

        public void Dispose()
        {
            TodoEventDispatcher.TodoUpdated -= async (sender, integrationEvent) => await HandleAsync(integrationEvent);
        }
    }
}
