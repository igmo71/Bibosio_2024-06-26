namespace Bibosio.WebApi.Interfaces
{
    public interface IEventHandler<in TEvent> : IHandler where TEvent : IIntegrationEvent
    {
        //void Handle(TEvent integrationEvent);
        Task HandleAsync(TEvent integrationEvent);
    }
}
