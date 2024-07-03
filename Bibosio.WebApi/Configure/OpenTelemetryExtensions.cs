using Bibosio.WebApi.Common;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Bibosio.WebApi.Configure
{
    public static class OpenTelemetryExtensions
    {
        public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services)
        {
            string serviceName = "Bibosio.WebApi";

            services.AddSingleton<AppInstrumentation>();

            services.AddOpenTelemetry()
                .ConfigureResource(resourceBuilder => resourceBuilder
                    .AddService(serviceName))
                .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                    .AddSource(serviceName)
                    .AddSource(AppInstrumentation.ActivitySourceName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddOtlpExporter(exporterOptions =>
                    {
                        exporterOptions.Endpoint = new Uri("http://igmo-pc:5341/ingest/otlp/v1/traces");
                        exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                        exporterOptions.Headers = "X-Seq-ApiKey=x4d4zxG37lHw9bSxP74B";
                    }))
                .WithMetrics(meterProviderBuilder => meterProviderBuilder
                    .AddMeter(AppInstrumentation.MeterName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter())
                .WithLogging(loggerProviderBuilder =>
                {
                    loggerProviderBuilder
                        .AddConsoleExporter()
                        .AddOtlpExporter(exporterOptions =>
                        {
                            exporterOptions.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
                            exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                            exporterOptions.Headers = "X-Seq-ApiKey=x4d4zxG37lHw9bSxP74B";
                        });
                });

            return services;
        }
    }
}
