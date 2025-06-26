using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;

public record Client(string Document, string Fullname, Phone? Phone, Email? Email, Address? Address) : Entity
{
    private Client() : this(string.Empty, string.Empty, null, null, null) { }

    public string Document { get; private set; } = Document;
    public string Fullname { get; private set; } = Fullname;
    public Phone? Phone { get; private set; } = Phone;
    public Email? Email { get; private set; } = Email;
    public Address? Address { get; private set; } = Address;

    public Client Update(string fullname, string document, string email, Phone? phone, Address? address)
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
        Phone = new Phone(phone.CountryCode, phone.AreaCode, phone.Number, phone.Type);
    }

    private void UpdateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) return;
        Email = email;
    }

    private void UpdateAddress(Address? address)
    {
        if (address is null) return;
        Address = new Address(address.Street, address.City, address.State, address.ZipCode);
    }
}
