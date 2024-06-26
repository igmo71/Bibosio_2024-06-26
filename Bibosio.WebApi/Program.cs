using Bibosio.WebApi.Common;
using Bibosio.WebApi.Data;
using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Diagnostics;

namespace Bibosio.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            string serviceName = builder.Environment.ApplicationName;
            ActivitySource activitySource = new(serviceName);

            builder.Services.AddSingleton<AppInstrumentation>();

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithTracing(tracing => tracing
                    .AddSource(activitySource.Name)
                    .AddSource(AppInstrumentation.ActivitySourceName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri("http://igmo-pc:5341/ingest/otlp/v1/traces");
                        options.Protocol = OtlpExportProtocol.HttpProtobuf;
                        options.Headers = "X-Seq-ApiKey=x4d4zxG37lHw9bSxP74B";
                    }))
                .WithMetrics(metrics => metrics
                    .AddMeter(AppInstrumentation.MeterName)
                    //.AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
                );


            builder.Services.AddSerilog(options =>
            {
                options
                    .ReadFrom.Configuration(builder.Configuration)
                    .WriteTo.Seq(serverUrl: "http://igmo-pc:5341", apiKey: "x4d4zxG37lHw9bSxP74B");
            });

            //builder.Services.AddSqlite<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
            //builder.Services.AddSqlServer<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                //.LogTo(Log.Debug)
                );
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<InMemoryMessageQueue>();
            builder.Services.AddSingleton<IEventBus, EventBus>();
            builder.Services.AddHostedService<IntegrationEventProcessorJob>();

            TodoModule.Register(builder.Services);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseSerilogRequestLogging();

            TodoModule.MapEndpoints(app);

            app.Run();
        }
    }
}
