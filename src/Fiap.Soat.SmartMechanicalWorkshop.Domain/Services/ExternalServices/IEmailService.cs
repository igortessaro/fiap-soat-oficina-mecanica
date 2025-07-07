namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.ExternalServices;
public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string bodyHtml);
}
