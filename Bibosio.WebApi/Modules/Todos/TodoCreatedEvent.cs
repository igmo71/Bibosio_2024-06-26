using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos.Models;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoCreatedEvent : Todo, IIntegrationEvent
    {
        public Guid EventId { get; init; }
    }
}
