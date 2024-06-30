using Bibosio.WebApi.Modules.Todos.EventHandlers.Handlers;

namespace Bibosio.WebApi.Modules.Todos.EventHandlers
{
    public class TodoEventHandler : BackgroundService
    {
        private readonly ILogger<TodoEventHandler> _logger;

        private readonly TodoCreatedEventHandler _todoCreatedEventHandler;
        private readonly TodoUpdatedEventHandler _todoUpdatedEventHandler;

        public TodoEventHandler(ILogger<TodoEventHandler> logger,
            TodoCreatedEventHandler todoCreatedEventHandler,
            TodoUpdatedEventHandler todoUpdatedEventHandler)
        {
            _logger = logger;

            _todoCreatedEventHandler = todoCreatedEventHandler;
            _todoUpdatedEventHandler = todoUpdatedEventHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}
