using System.Net.Mail;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string from, string to, string subject, string body, bool isHtml = false);
    Task SendEmailAsync(MailAddress from, MailAddress to, string subject, string body, bool isHtml = false);
}
