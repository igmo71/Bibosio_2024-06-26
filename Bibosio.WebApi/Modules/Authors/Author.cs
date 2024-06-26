using Bibosio.WebApi.Common;

namespace Bibosio.WebApi.Modules.Authors
{
    public class Author : EntityBase
    {
        public required string Name { get; set; }
    }
}
