using Bibosio.WebApi.Modules.Todos.Models;
using Microsoft.EntityFrameworkCore;

namespace Bibosio.WebApi.Modules.Todos.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {}

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TodoEntityConfiguration());
        }
    }
}
