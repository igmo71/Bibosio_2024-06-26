using System.Threading.Channels;

namespace Bibosio.WebApi.Modules.Todos
{
    public interface ITodoEventChannel
    {
        ChannelReader<TodoCreatedEvent> Reader { get; set; }
        ChannelWriter<TodoCreatedEvent> Writer { get; set; }

        ValueTask Write(TodoCreatedEvent todoEvent);
    }
}
