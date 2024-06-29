using System.Threading.Channels;

namespace Bibosio.WebApi.Interfaces
{
    public interface IEventChannel
    {
        ChannelWriter<IIntegrationEvent> Writer { get; }
        ChannelReader<IIntegrationEvent> Reader { get; }
    }
}
