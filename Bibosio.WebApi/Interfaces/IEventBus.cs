namespace Bibosio.WebApi.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync<T>(
        T integrationEvent,
        CancellationToken cancellationToken = default)
        where T : class, IIntegrationEvent;
    }
}
