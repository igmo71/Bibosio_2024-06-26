using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Bibosio.WebApi.Common
{
    public class AppInstrumentation
    {
        internal static string ActivitySourceName = $"{nameof(Bibosio)}.{nameof(WebApi)}.AppActivitySource";
        internal const string MeterName = $"{nameof(Bibosio)}.{nameof(WebApi)}.AppMeter";
        //private readonly Meter meter;

        public AppInstrumentation()
        {
            string? version = typeof(AppInstrumentation).Assembly.GetName().Version?.ToString();

            ActivitySource = new ActivitySource(ActivitySourceName, version);
            Meter = new Meter(MeterName, version);
            Counters = new();
            TodoCreatedCounter = Meter.CreateCounter<long>("todo.created", description: "The number of Todo Created");
        }

        public ActivitySource ActivitySource { get; }

        public Meter Meter { get; }

        public Dictionary<string, object> Counters { get; set; }

        public void AddCounter<T>(string counterName) where T : struct
        {
            if (!Counters.ContainsKey(counterName))
            {
                Counters.Add(counterName, Meter.CreateCounter<T>(counterName));
            }
        }

        public Counter<long> TodoCreatedCounter { get; }

        public void Dispose()
        {
            this.ActivitySource.Dispose();
            this.Meter.Dispose();
        }
    }
}
