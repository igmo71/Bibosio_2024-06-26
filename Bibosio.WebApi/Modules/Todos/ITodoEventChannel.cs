using System.Threading.Channels;

namespace Bibosio.WebApi.Modules.Todos
{
    public interface ITodoEventChannel
    {
        ChannelReader<TodoEvent> Reader { get; set; }
        ChannelWriter<TodoEvent> Writer { get; set; }

        ValueTask Write(TodoEvent todoEvent);
    }
}
