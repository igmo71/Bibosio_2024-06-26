using Bibosio.WebApi.Interfaces;

namespace Bibosio.WebApi.Common
{
    public abstract class EntityBase : IEntityBase<Guid>
    {
        public Guid Id { get; set; }
    }
}
