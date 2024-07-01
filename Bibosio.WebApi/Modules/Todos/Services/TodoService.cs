using Bibosio.WebApi.Common;
using Bibosio.WebApi.Modules.Todos.Data;
using Bibosio.WebApi.Modules.Todos.Interfaces;
using Bibosio.WebApi.Modules.Todos.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Bibosio.WebApi.Modules.Todos.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoDbContext _dbContext;
        private readonly ITodoEventBus _eventBus;
        private readonly ILogger<TodoService> _logger;

        private readonly AppInstrumentation _appInstrumentation;
        private readonly ActivitySource _activitySource;
        private readonly string todoCreatedCount = "todoCreatedCount";

        public TodoService(
            TodoDbContext dbContext,
            ITodoEventBus eventBus,
            ILogger<TodoService> logger,
            AppInstrumentation appInstrumentation,
            TodoCounter counter)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
            _logger = logger;

            _appInstrumentation = appInstrumentation;
            _activitySource = appInstrumentation.ActivitySource;
            _appInstrumentation.CreateCounter<int>(todoCreatedCount);
        }

        public async Task<IResult> Create(Todo todo)
        {
            using var activity = _activitySource.StartActivity("CreateTodoActivity");

            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            ((Counter<int>)_appInstrumentation.Counters[todoCreatedCount])?.Add(1);

            _logger.LogDebug("{Method} {@Todo}", nameof(Create), todo);

            await _eventBus.PublishTodoCreated(todo);

            return TypedResults.Created($"/todoitems/{todo.Id}", todo);
        }

        public async Task<IResult> Delete(int id)
        {
            if (await _dbContext.Todos.FindAsync(id) is Todo todo)
            {
                _dbContext.Todos.Remove(todo);
                await _dbContext.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        public async Task<IResult> Get(int id)
        {
            return await _dbContext.Todos.FindAsync(id)
                is Todo todo
                    ? TypedResults.Ok(todo)
                    : TypedResults.NotFound();
        }

        public async Task<IResult> GetAll()
        {
            _logger.LogDebug(nameof(GetAll));
            return TypedResults.Ok(await _dbContext.Todos.ToArrayAsync());
        }

        public async Task<IResult> GetComplete()
        {
            return TypedResults.Ok(await _dbContext.Todos.Where(t => t.IsComplete).ToListAsync());
        }

        public async Task<IResult> Update(int id, Todo inputTodo)
        {
            var todo = await _dbContext.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;

            await _dbContext.SaveChangesAsync();

            _logger.LogDebug("{Method} {@Todo}", nameof(Update), todo);

            await _eventBus.PublishTodoUpdated(todo);

            return TypedResults.NoContent();
        }
    }
}
