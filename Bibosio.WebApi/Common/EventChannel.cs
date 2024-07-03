using System.Threading.Channels;
using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    public abstract class EventChannel : IEventChannel
    {
        protected readonly Channel<IIntegrationEvent> _channel = Channel.CreateUnbounded<IIntegrationEvent>();
        public ChannelWriter<IIntegrationEvent> Writer => _channel.Writer;
        public ChannelReader<IIntegrationEvent> Reader => _channel.Reader;
    }
}
