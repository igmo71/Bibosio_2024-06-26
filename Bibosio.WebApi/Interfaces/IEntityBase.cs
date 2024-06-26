namespace Bibosio.WebApi.Interfaces
{
    public interface IEntityBase<TId>
    {
        public TId Id { get; set; }
    }
}
