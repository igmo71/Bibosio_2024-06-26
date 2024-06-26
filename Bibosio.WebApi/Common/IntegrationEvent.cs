    using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    public abstract record IntegrationEvent(Guid EventId) : IIntegrationEvent;
}
