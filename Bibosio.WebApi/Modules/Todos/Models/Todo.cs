using Bibosio.WebApi.Common;

namespace Bibosio.WebApi.Modules.Todos.Models
{
    public class Todo : EntityBase
    {
        public new int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
