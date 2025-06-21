namespace AutoRepairShopManagementSystem.Repositories
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositoryExtensions(this IServiceCollection iServiceCollection)
        {

            iServiceCollection.AddScoped<ClientRepository>();
            iServiceCollection.AddScoped<VehicleRepository>();


            return iServiceCollection;

        }
    }
}
