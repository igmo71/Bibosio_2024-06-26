using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoCreatedEvent : Todo, IIntegrationEvent
    {
        public Guid EventId { get; init; }
    }
}
