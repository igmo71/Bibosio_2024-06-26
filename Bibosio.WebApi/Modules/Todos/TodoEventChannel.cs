using System.Threading.Channels;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoEventChannel : ITodoEventChannel
    {
        public TodoEventChannel()
        {
            var channel = Channel.CreateUnbounded<TodoEvent>();
            Reader = channel.Reader;
            Writer = channel.Writer;
        }

        public ChannelReader<TodoEvent> Reader { get; set; }
        public ChannelWriter<TodoEvent> Writer { get; set; }

        public async ValueTask Write(TodoEvent todoEvent)
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
