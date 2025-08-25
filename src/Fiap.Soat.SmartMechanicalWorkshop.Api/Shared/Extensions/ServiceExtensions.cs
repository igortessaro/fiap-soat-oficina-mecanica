using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddServiceExtensions(this IServiceCollection serviceCollection)
    {
        _ = serviceCollection.AddSingleton<IEmailService, EmailService>();
        _ = serviceCollection.AddSingleton<IEmailTemplateProvider, EmailTemplateProvider>();

        return serviceCollection;
    }
}
