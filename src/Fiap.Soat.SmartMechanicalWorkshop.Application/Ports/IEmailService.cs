namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Ports;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string bodyHtml);
}
