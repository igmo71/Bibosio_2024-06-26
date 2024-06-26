using Microsoft.EntityFrameworkCore;

namespace Bibosio.WebApi.Modules.Todos.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            
        }
    }
}
