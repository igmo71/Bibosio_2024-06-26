using Bibosio.WebApi.Modules.Blogs.Posts;

namespace Bibosio.WebApi.Modules.Blogs
{
    public class BlogAggregate
    {
        public required Blog Blog { get; set; }
        public Post? Post { get; set; }
    }
}
