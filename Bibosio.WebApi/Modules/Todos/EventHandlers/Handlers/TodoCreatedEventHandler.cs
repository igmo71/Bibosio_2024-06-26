using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos.EventBus;
using Bibosio.WebApi.Modules.Todos.EventBus.Events;

namespace Bibosio.WebApi.Modules.Todos.EventHandlers.Handlers
{
    public class TodoCreatedEventHandler : IEventHandler<TodoCreatedEvent>, IDisposable
    {
        private readonly ILogger<TodoCreatedEventHandler> _logger;

        public TodoCreatedEventHandler(ILogger<TodoCreatedEventHandler> logger)
        {
            TodoEventDispatcher.TodoCreated += async (sender, integrationEvent) => await HandleAsync(integrationEvent);
            _logger = logger;
        }

        public async Task HandleAsync(TodoCreatedEvent integrationEvent)
        {
            await Task.Delay(1000); // TODO: Remove in prod
            _logger.LogDebug("{Object} {Method} {@TodoCreatedEvent}", nameof(TodoCreatedEventHandler), nameof(HandleAsync), integrationEvent);
        }

        public void Dispose()
        {
            TodoEventDispatcher.TodoCreated -= async (sender, integrationEvent) => await HandleAsync(integrationEvent);
        }
    }
}
