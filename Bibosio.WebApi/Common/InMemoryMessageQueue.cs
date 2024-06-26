using System.Threading.Channels;
using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    internal sealed class InMemoryMessageQueue
    {
        private readonly Channel<IIntegrationEvent> _channel =
            Channel.CreateUnbounded<IIntegrationEvent>();

        public ChannelReader<IIntegrationEvent> Reader => _channel.Reader;

        public ChannelWriter<IIntegrationEvent> Writer => _channel.Writer;
    }

}
