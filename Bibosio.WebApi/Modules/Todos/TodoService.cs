using Bibosio.WebApi.Common;
using Bibosio.WebApi.Data;
using Bibosio.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _dbContext;
        private readonly IEventBus _eventBus;
        private readonly ILogger<TodoService> _logger;
        private readonly ActivitySource _activitySource;
        private readonly TodoCounter _counter;
        private readonly Counter<long> _todoCreatedCounter;

        public TodoService(
            AppDbContext dbContext,
        IEventBus eventBus,
        ILogger<TodoService> logger,
        AppInstrumentation appInstrumentation,
        TodoCounter counter)
        {
            _dbContext = dbContext;
            _eventBus = eventBus;
            _logger = logger;
            _activitySource = appInstrumentation.ActivitySource;
            _counter = counter;
            _todoCreatedCounter = appInstrumentation.TodoCreatedCounter;
        }

        public async Task<IResult> Create(Todo todo)
        {
            using var activity = _activitySource.StartActivity("CreateTodoActivity");

            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            _counter.TodoCreatedCounter.Add(1);
            _todoCreatedCounter.Add(1);

            _logger.LogDebug("{Method} {@Todo}", nameof(Create), todo);

            await PublishTodoCreated(todo);

            return TypedResults.Created($"/todoitems/{todo.Id}", todo);

        }

        private async Task PublishTodoCreated(Todo todo)
        {
            var todoEvent = new TodoCreatedEvent
            {
                EventId = Guid.NewGuid(),
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete
            };
            await _eventBus.PublishAsync<TodoCreatedEvent>(todoEvent);
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

            return TypedResults.NoContent();
        }
    }
}
