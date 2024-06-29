using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos.Models;

namespace Bibosio.WebApi.Modules.Todos.Interfaces
{
    public interface ITodoEventBus : IEventBus
    {
        Task PublishTodoCreated(Todo todo);
        Task PublishTodoUpdated(Todo todo);
    }
}
