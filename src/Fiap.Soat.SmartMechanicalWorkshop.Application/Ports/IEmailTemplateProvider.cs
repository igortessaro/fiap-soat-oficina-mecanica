using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Ports;

public interface IEmailTemplateProvider
{
    string GetTemplate(ServiceOrder serviceOrder);
}
