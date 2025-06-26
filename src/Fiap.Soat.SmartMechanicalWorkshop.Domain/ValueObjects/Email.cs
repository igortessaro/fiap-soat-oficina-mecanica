namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

public record Email(string Address)
{
    public string Address { get; private set; } = Address;

    public static implicit operator Email(string address) => new Email(address);
    public static implicit operator string(Email email) => email.Address;
}
