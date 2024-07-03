using Bibosio.WebApi.Common;
using Bibosio.WebApi.Configure;
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

            builder.Services.ConfigureOpenTelemetry();

            builder.Services.AddSerilog(options => options
                    .ReadFrom.Configuration(builder.Configuration)
                    .WriteTo.Seq(serverUrl: "http://igmo-pc:5341", apiKey: "x4d4zxG37lHw9bSxP74B"));

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
    }
}
