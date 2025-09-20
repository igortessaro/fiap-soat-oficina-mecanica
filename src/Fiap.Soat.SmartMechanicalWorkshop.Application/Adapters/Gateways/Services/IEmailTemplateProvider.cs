using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Services;

public interface IEmailTemplateProvider
{
    string GetTemplate(ServiceOrder serviceOrder);
}
