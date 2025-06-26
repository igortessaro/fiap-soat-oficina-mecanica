namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

public record Address(string Street, string City, string State, string ZipCode)
{
    public string Street { get; private set; } = Street;
    public string City { get; private set; } = City;
    public string State { get; private set; } = State;
    public string ZipCode { get; private set; } = ZipCode;
}