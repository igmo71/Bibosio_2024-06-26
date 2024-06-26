using System.Threading.Channels;

namespace Bibosio.WebApi.Modules.Todos
{
    public interface ITodoEventChannel
    {
        ChannelReader<TodoCreated> Reader { get; set; }
        ChannelWriter<TodoCreated> Writer { get; set; }

        ValueTask Write(TodoCreated todoEvent);
    }
}
