namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

public record Phone(string CountryCode, string AreaCode, string Number, string Type)
{
    public string CountryCode { get; private set; } = CountryCode;
    public string AreaCode { get; private set; } = AreaCode;
    public string Number { get; private set; } = Number;
    public string Type { get; private set; } = Type;
}
