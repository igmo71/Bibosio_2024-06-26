using System.Threading.Channels;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoEventChannel : ITodoEventChannel
    {
        public TodoEventChannel()
        {
            var channel = Channel.CreateUnbounded<TodoCreated>();
            Reader = channel.Reader;
            Writer = channel.Writer;
        }

        public ChannelReader<TodoCreated> Reader { get; set; }
        public ChannelWriter<TodoCreated> Writer { get; set; }

        public async ValueTask Write(TodoCreated todoEvent)
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
