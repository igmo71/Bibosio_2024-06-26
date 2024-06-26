using System.Threading.Channels;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoEventChannel : ITodoEventChannel
    {
        public TodoEventChannel()
        {
            var channel = Channel.CreateUnbounded<TodoCreatedEvent>();
            Reader = channel.Reader;
            Writer = channel.Writer;
        }

        public ChannelReader<TodoCreatedEvent> Reader { get; set; }
        public ChannelWriter<TodoCreatedEvent> Writer { get; set; }

        public async ValueTask Write(TodoCreatedEvent todoEvent)
        {
            while (await Writer.WaitToWriteAsync())
            {

                if (Writer.TryWrite(item: todoEvent))
                {
                    Console.WriteLine("Write Todo Event");
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
            }
        }


    }
}
