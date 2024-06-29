using Bibosio.WebApi.Modules.Todos.EventBus.Events;
using Bibosio.WebApi.Modules.Todos.Interfaces;
using Bibosio.WebApi.Modules.Todos.Models;

namespace Bibosio.WebApi.Modules.Todos.EventBus
{
    internal class TodoEventBus : Common.EventBus, ITodoEventBus
    {
        public TodoEventBus(ITodoEventChannel eventChannel, ILogger<TodoEventBus> logger) : base(eventChannel, logger)
        { }

        public async Task PublishTodoCreated(Todo todo)
        {
            var todoCreatedEvent = new TodoCreatedEvent
            {
                EventId = Guid.NewGuid(),
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete
            };
            await PublishAsync(todoCreatedEvent);
        }

        public async Task PublishTodoUpdated(Todo todo)
        {
            var todoUpdatedEvent = new TodoUpdatedEvent
            {
                EventId = Guid.NewGuid(),
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete
            };
            await PublishAsync(todoUpdatedEvent);
        }
    }
}
