using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoEvent : Todo, IIntegrationEvent
    {
        public Guid EventId { get; init; }
    }
}
