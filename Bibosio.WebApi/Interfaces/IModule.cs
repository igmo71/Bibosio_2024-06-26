namespace Bibosio.WebApi.Interfaces
{
    public interface IModule
    {
        abstract static IServiceCollection Register(IServiceCollection services);
        abstract static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}
