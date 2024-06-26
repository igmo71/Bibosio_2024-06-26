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
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();



            var builder = WebApplication.CreateBuilder(args);

            //string serviceName = builder.Environment.ApplicationName;
            //ActivitySource activitySource = new(serviceName);

            string serviceName = "BibosioServiceName";
            ActivitySource activitySource = new("BibosioActivitySource");

            builder.Services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithTracing(tracing => tracing
                    .AddSource(activitySource.Name)
                    .AddSource(Instrumentation.ActivitySourceName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddConsoleExporter()
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/traces");
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        opt.Headers = "X-Seq-ApiKey=x4d4zxG37lHw9bSxP74B";
                    }))
                .WithMetrics(metrics => metrics
                .AddMeter(Instrumentation.MeterName)
                //    .AddAspNetCoreInstrumentation()
                //    .AddConsoleExporter()
                );

            builder.Services.AddSingleton<Instrumentation>();

            builder.Services.AddSerilog(options =>
            {
                options.ReadFrom.Configuration(builder.Configuration);
            });
            Log.Information("Starting up");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    //.LogTo(Log.Debug)
                    ;
            });
            //builder.Services.AddSqlServer<AppDbContext>(connectionString);
            //builder.Services.AddSqlite<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));
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
