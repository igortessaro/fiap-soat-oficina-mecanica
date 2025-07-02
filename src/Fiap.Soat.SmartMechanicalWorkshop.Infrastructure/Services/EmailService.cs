using Amazon.SimpleEmail;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.ExternalServices;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly AmazonSimpleEmailServiceClient _sesClient;
    private readonly string _fromAddress;

    public EmailService(IConfiguration config)
    {
        string? awsAccessKey = config["AWS:AccessKey"];
        string? awsSecretKey = config["AWS:SecretKey"];
        string? awsRegion = config["AWS:Region"];
        _fromAddress = config["AWS:FromAddress"];

        var sesConfig = new AmazonSimpleEmailServiceConfig
        {
            ServiceURL = config["AWS:ServiceUrl"] ?? "http://localhost:4566", // Padrão do LocalStack

        };

        _sesClient = new AmazonSimpleEmailServiceClient(awsAccessKey, awsSecretKey, sesConfig);
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string htmlBody)
    {
        var fromAddress = new MailAddress("noreply@smartworkshop.com", "Oficina");
        var toAddress = new MailAddress(to);
        const string smtpHost = "localhost";
        const int smtpPort = 1025;

        using var smtp = new SmtpClient
        {
            Host = smtpHost,
            Port = smtpPort,
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = true
        };

        using var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

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


