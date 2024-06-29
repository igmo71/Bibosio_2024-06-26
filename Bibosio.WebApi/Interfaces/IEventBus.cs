namespace Bibosio.WebApi.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = default) 
            where TEvent : class, IIntegrationEvent;
    }
}
