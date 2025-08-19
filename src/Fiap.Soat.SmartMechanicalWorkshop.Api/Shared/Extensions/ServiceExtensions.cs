using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services.Interfaces;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServiceExtensions(this IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddTransient<ISupplyService, SupplyService>();
        _ = serviceCollection.AddSingleton<IEmailService, EmailService>();
        _ = serviceCollection.AddSingleton<IEmailTemplateProvider, EmailTemplateProvider>();
        _ = serviceCollection.AddTransient<IQuoteService, QuoteService>();

        return serviceCollection;
    }
}
