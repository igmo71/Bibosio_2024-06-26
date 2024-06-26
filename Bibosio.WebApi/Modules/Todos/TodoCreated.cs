using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoCreated : Todo, IIntegrationEvent
    {
        public Guid EventId { get; init; }
    }
}
