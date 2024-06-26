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
            TodoCreatedCounter = Meter.CreateCounter<long>("todo.created", description: "The number of Todo Created");
        }

        public ActivitySource ActivitySource { get; }

        public Meter Meter { get; }

        public Counter<long> TodoCreatedCounter { get; }

        public void Dispose()
        {
            this.ActivitySource.Dispose();
            this.Meter.Dispose();
        }
    }
}
