using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;

public record Client : Entity
{
    private Client() { }

    public string Document { get; private set; } = string.Empty;
    public string Fullname { get; private set; } = string.Empty;
    public Phone Phone { get; private set; }
    public Email Email { get; private set; }
    public Guid AddressId { get; private set; }
    public Address Address { get; private set; }
    public IReadOnlyList<Vehicle> Vehicles { get; private set; } = [];

    public Client Update(string fullname, string document, string email, Phone phone, Address? address)
    {
        if (!string.IsNullOrEmpty(document)) Document = document;
        if (!string.IsNullOrEmpty(fullname)) Fullname = fullname;
        UpdatePhone(phone);
        UpdateEmail(email);
        UpdateAddress(address);
        return this;
    }

    private void UpdatePhone(Phone? phone)
    {
        if (phone is null) return;
        Phone = new Phone(phone.AreaCode, phone.Number);
    }

    private void UpdateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return;
        Email = email;
    }

    private void UpdateAddress(Address? address) => Address.Update(address);
}
