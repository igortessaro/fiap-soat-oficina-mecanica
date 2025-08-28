using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services;

public class EmailService(IOptions<EmailSettings> options, IEmailSender emailSender) : IEmailService
{
    private readonly EmailSettings _emailSettings = options.Value;

    public async Task SendEmailAsync(string to, string subject, string bodyHtml)
    {
        var fromAddress = new MailAddress(_emailSettings.SenderAddress, _emailSettings.SenderName);
        var toAddress = new MailAddress(to);
        await emailSender.SendEmailAsync(fromAddress, toAddress, subject, bodyHtml, true);
    }
}
