using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Bibosio.WebApi.Common
{
    public class Instrumentation
    {
        internal const string ActivitySourceName = "Bibosio.Instrumentation.ActivitySource";
        internal const string MeterName = "Bibosio.Instrumentation.Meter";
        private readonly Meter meter;

        public Instrumentation()
        {
            string? version = typeof(Instrumentation).Assembly.GetName().Version?.ToString();
            this.ActivitySource = new ActivitySource(ActivitySourceName, version);
            this.meter = new Meter(MeterName, version);
            this.TodoCreatedCounter = this.meter.CreateCounter<long>("todo.created", description: "The number of Todo Created");
        }

        public ActivitySource ActivitySource { get; }

        public Counter<long> TodoCreatedCounter { get; }

        public void Dispose()
        {
            this.ActivitySource.Dispose();
            this.meter.Dispose();
        }
    }
}
