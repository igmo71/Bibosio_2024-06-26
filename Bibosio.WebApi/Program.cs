using Bibosio.WebApi.Common;
using Bibosio.WebApi.Data;
using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
