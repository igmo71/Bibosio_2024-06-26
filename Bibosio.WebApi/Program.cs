using Bibosio.WebApi.Common;
using Bibosio.WebApi.Data;
using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos;
using Bibosio.WebApi.Modules.Todos.EventBus;
using Bibosio.WebApi.Modules.Todos.EventBus.Events;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
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
                .ConfigureResource(resource => resource
                    .AddService(serviceName))
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
                    .AddConsoleExporter())
                .WithLogging(logging =>
                {
                    logging
                        .AddConsoleExporter()
                        .AddOtlpExporter(exporterOptions =>
                        {
                            exporterOptions.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
                            exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                            exporterOptions.Headers = "X-Seq-ApiKey=x4d4zxG37lHw9bSxP74B";
                        });
                })
                ;

            //builder.Services.AddLogging(logging => logging.AddOpenTelemetry(openTelemetryLoggerOptions =>
            //{
            //    openTelemetryLoggerOptions.SetResourceBuilder(
            //        ResourceBuilder.CreateEmpty()
            //            .AddService(serviceName)
            //            .AddAttributes(new Dictionary<string, object>
            //            {
            //                ["deployment.environment"] = "development"
            //            }));
            //    openTelemetryLoggerOptions.IncludeScopes = true;
            //    openTelemetryLoggerOptions.IncludeFormattedMessage = true;

            //    openTelemetryLoggerOptions.AddOtlpExporter(exporter =>
            //    {
            //        exporter.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
            //        exporter.Protocol = OtlpExportProtocol.HttpProtobuf;
            //        exporter.Headers = "X-Seq-ApiKey=x4d4zxG37lHw9bSxP74B";
            //    });
            //}));

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

            builder.Services.AddSingleton<IEventChannel, AppEventChannel>();
            builder.Services.AddSingleton<IEventBus, AppEventBus>();
            builder.Services.AddHostedService<AppEventDispatcher>();

            TodoEventDispatcher.TodoCreated += async (object? sender, TodoCreatedEvent e) => await TodoCreatedHandle1(sender, e);
            TodoEventDispatcher.TodoCreated += async (object? sender, TodoCreatedEvent e) => await TodoCreatedHandle2(sender, e);
            TodoEventDispatcher.TodoUpdated += async(object? sender, TodoUpdatedEvent e) => await TodoUpdatedHandle(sender, e);



            TodoModule.Register(builder.Services, builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();

            TodoModule.MapEndpoints(app);

            app.Run();
        }

        private static async Task TodoCreatedHandle1(object? sender, TodoCreatedEvent e)
        {
            await Task.Delay(1000);
            Log.Debug("{Method} {@TodoCreatedEvent} ", nameof(TodoCreatedHandle1), e);
            await Task.Delay(1000);
        }

        private static async Task TodoCreatedHandle2(object? sender, TodoCreatedEvent e)
        {
            await Task.Delay(1000);
            Log.Debug("{Method} {@TodoCreatedEvent} ", nameof(TodoCreatedHandle2), e);
            await Task.Delay(1000);
        }


        private static async Task TodoUpdatedHandle(object? sender, TodoUpdatedEvent e)
        {
            await Task.Delay(1000);
            Log.Debug("{Method} {@TodoUpdatedEvent} {@Sender}", nameof(TodoUpdatedHandle), e, sender);
        }
    }
}
