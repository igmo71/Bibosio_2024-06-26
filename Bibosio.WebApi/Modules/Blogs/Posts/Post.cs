using Bibosio.WebApi.Common;

namespace Bibosio.WebApi.Modules.Blogs.Posts
{
    public class Post : EntityBase
    {
        public required string Title { get; set; }

        public string? Article { get; set; }
    }
}
