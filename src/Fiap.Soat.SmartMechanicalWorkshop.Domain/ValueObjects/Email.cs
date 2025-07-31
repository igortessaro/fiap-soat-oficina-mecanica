namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

public record Email
{
    private Email() { }

    private Email(string address)
    {
        Address = address;
    }

    public string Address { get; private set; } = string.Empty;

    public static implicit operator Email(string address) => new Email(address);
    public static implicit operator string(Email email) => email.Address;
}
