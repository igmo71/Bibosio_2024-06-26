using Bibosio.WebApi.Modules.Todos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bibosio.WebApi.Modules.Todos.Data
{
    public class TodoEntityConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable(name: "Todos", schema:"todo");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100);
        }
    }
}
