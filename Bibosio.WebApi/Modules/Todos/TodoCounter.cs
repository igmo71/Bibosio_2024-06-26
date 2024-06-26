using Bibosio.WebApi.Common;
using System.Diagnostics.Metrics;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoCounter
    {
        public TodoCounter(AppInstrumentation appInstrumentation)
        {
            TodoCreatedCounter = appInstrumentation.Meter.CreateCounter<long>("-------------------->>>todo.created", description: ">>>>>>>The number of Todo Created");
        }

        public Counter<long> TodoCreatedCounter { get; }
    }
}
