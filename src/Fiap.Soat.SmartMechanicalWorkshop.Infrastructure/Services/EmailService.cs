using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _emailSettings = options.Value;

    public async Task<bool> SendEmailAsync(string to, string subject, string htmlBody)
    {
        var fromAddress = new MailAddress(_emailSettings.SenderAddress, _emailSettings.SenderName);
        var toAddress = new MailAddress(to);

        using var smtp = new SmtpClient
        {
            Host = _emailSettings.SmtpHost,
            Port = _emailSettings.SmtpPort,
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = true
        };

        using var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = htmlBody, IsBodyHtml = true };

        try
        {
            await smtp.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            return false;
        }
    }
}
