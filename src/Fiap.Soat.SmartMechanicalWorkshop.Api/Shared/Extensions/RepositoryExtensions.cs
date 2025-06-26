using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositoryExtensions(this IServiceCollection iServiceCollection)
        {
            iServiceCollection.AddScoped<VehicleRepository>();
            iServiceCollection.AddScoped<SupplyRepository>();
            iServiceCollection.AddScoped<AvailableServiceRepository>();
            iServiceCollection.AddScoped<IClientRepository, ClientRepository>();

            return iServiceCollection;

        }
    }
}
