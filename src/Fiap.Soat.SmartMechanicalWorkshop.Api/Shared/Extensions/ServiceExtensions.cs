using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection iServiceCollection)
        {
            iServiceCollection.AddTransient<IVehicleService, VehicleService>();
            iServiceCollection.AddTransient<ISupplyService, SupplyService>();
            iServiceCollection.AddTransient<IAvailableService, AvailableServiceService>();
            iServiceCollection.AddTransient<IClientService, ClientService>();

            return iServiceCollection;

        }
    }
}
