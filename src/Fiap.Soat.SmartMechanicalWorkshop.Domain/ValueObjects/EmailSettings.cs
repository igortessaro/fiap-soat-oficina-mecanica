namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
public class EmailSettings
{
    public string SenderAddress { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; }
}
