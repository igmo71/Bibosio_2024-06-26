using Bibosio.WebApi.Common;
using Bibosio.WebApi.Modules.Authors;

namespace Bibosio.WebApi.Modules.Blogs
{
    public class Blog : EntityBase
    {
        public Author? Author { get; set; }
    }
}
