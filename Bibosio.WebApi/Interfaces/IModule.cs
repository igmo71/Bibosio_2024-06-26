namespace Bibosio.WebApi.Interfaces
{
    public interface IModule
    {
        abstract static IServiceCollection Register(IServiceCollection services, IConfiguration configuration);
        abstract static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}
