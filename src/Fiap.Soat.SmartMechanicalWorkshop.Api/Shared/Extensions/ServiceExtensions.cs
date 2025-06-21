using AutoRepairShopManagementSystem.Services.Interfaces;

namespace AutoRepairShopManagementSystem.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServiceExtensions(this IServiceCollection iServiceCollection)
        {

            iServiceCollection.AddTransient<IClientService, ClientService>();
            iServiceCollection.AddTransient<IVehicleService, VehicleService>();


            return iServiceCollection;

        }
    }
}
