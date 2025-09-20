namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string bodyHtml);
}
