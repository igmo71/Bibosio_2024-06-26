using Bibosio.WebApi.Common;
using Bibosio.WebApi.Data;
using Bibosio.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Bibosio.WebApi.Modules.Todos
{
    public class TodoService(AppDbContext dbContext, IEventBus eventBus, ILogger<TodoService> logger) : ITodoService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IEventBus _eventBus = eventBus;
        private readonly ILogger<TodoService> _logger = logger;

        public async Task<IResult> Create(Todo todo)
        {
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            await _eventBus.PublishAsync<TodoEvent>(new TodoEvent
            {
                EventId = Guid.NewGuid(),
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete
            });

            _logger.LogDebug("{Method} {@Todo}", nameof(Create), todo);

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

            return TypedResults.NoContent();
        }
    }
}
