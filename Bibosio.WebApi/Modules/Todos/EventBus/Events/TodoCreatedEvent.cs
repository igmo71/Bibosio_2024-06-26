using Bibosio.WebApi.Common;
using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos.Models;

namespace Bibosio.WebApi.Modules.Todos.EventBus.Events
{
    public record TodoCreatedEvent(Guid EventId, Todo Todo) : IntegrationEvent(EventId)
    { }
}
