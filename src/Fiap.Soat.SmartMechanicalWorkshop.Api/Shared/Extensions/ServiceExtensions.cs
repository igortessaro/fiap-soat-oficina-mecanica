using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.ExternalServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServiceExtensions(this IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<IVehicleService, VehicleService>();
        _ = serviceCollection.AddTransient<ISupplyService, SupplyService>();
        _ = serviceCollection.AddTransient<IAvailableService, AvailableServiceService>();
        _ = serviceCollection.AddTransient<IPersonService, PersonService>();
        _ = serviceCollection.AddTransient<IServiceOrderService, ServiceOrderService>();
        _ = serviceCollection.AddSingleton<IEmailService, EmailService>();
        _ = serviceCollection.AddSingleton<IEmailTemplateProvider, EmailTemplateProvider>();
        _ = serviceCollection.AddTransient<IQuoteService, QuoteService>();
        _ = serviceCollection.AddTransient<IServiceOrderEventService, ServiceOrderEventService>();
        _ = serviceCollection.AddTransient<IAuthService, AuthService>();

        return serviceCollection;
    }
}
