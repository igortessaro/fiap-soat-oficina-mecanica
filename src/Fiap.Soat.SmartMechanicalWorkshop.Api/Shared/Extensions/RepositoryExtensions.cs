using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositoryExtensions(this IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddScoped<IAvailableServiceRepository, AvailableServiceRepository>();
        _ = serviceCollection.AddScoped<IServiceOrderRepository, ServiceOrderRepository>();
        _ = serviceCollection.AddScoped<ISupplyRepository, SupplyRepository>();
        _ = serviceCollection.AddScoped<IVehicleRepository, VehicleRepository>();
        _ = serviceCollection.AddScoped<IClientRepository, ClientRepository>();

        return serviceCollection;
    }
}
