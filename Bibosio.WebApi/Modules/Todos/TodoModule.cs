using Bibosio.WebApi.Interfaces;
using Bibosio.WebApi.Modules.Todos.Data;
using Bibosio.WebApi.Modules.Todos.Models;
using Bibosio.WebApi.Modules.Todos.Services;
using Microsoft.EntityFrameworkCore;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoModule : IModule
    {
        public static IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodoDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                );
            services.AddScoped<ITodoService, TodoService>();
            services.AddSingleton<ITodoEventChannel, TodoEventChannel>();
            services.AddSingleton<TodoCounter>();

            return services;
        }

        public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            var todoGroup = endpoints.MapGroup("/todos")
                .WithOpenApi();

            todoGroup.MapGet("/", async (ITodoService _todoService) => await _todoService.GetAll());
            todoGroup.MapGet("/complete", async (ITodoService _todoService) => await _todoService.GetComplete());
            todoGroup.MapGet("/{id}", async(int id, ITodoService _todoService) => await _todoService.Get(id));
            todoGroup.MapPost("/", async (Todo todo, ITodoService _todoService) => await _todoService.Create(todo));
            todoGroup.MapPut("/{id}", async (int id, Todo todo, ITodoService _todoService) => await _todoService.Update(id, todo));
            todoGroup.MapDelete("/{id}", async (int id, ITodoService _todoService) => await _todoService.Delete(id));

            return endpoints;
        }
    }
}
