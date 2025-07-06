using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
public interface IEmailTemplateProvider
{
    string GetTemplate(ServiceOrder serviceOrder);
}
