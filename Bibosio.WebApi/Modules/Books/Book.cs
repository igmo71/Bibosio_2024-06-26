using Bibosio.WebApi.Common;
using Bibosio.WebApi.Modules.Authors;

namespace Bibosio.WebApi.Modules.Books
{
    public class Book : EntityBase
    {
        public required string Title { get; set; }
        public required Author Author { get; set; }
        public string? Story { get; set; }
    }
}
