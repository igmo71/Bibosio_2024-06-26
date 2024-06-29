using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos.Models;

namespace Bibosio.WebApi.Modules.Todos.EventBus.Events
{
    public class TodoUpdatedEvent : Todo, IIntegrationEvent
    {
        public Guid EventId { get; init; }
    }
}
