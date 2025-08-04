namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string bodyHtml);
}
