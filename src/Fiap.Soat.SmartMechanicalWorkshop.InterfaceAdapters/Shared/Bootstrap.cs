using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Shared;

[ExcludeFromCodeCoverage]
public static class Bootstrap
{
    public static IServiceCollection AddInterfaceAdapters(this IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddScoped<IAvailableServicesController, AvailableServicesController>();
        _ = serviceCollection.AddScoped<IAuthenticationController, AuthenticationController>();
        _ = serviceCollection.AddScoped<IPeopleController, PeopleController>();
        _ = serviceCollection.AddScoped<IQuoteController, QuoteController>();
        _ = serviceCollection.AddScoped<IServiceOrdersController, ServiceOrdersController>();
        _ = serviceCollection.AddScoped<ISuppliesController, SuppliesController>();
        _ = serviceCollection.AddScoped<IVehiclesController, VehiclesController>();

        return serviceCollection;
    }
}
