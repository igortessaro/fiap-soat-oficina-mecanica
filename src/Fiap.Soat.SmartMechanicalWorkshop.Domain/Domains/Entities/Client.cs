using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;

public record Client(string Document, string Fullname, Phone? Phone) : Entity//, Phone? Phone, Email? Email, Address? Address) : Entity
{
    private Client() : this(string.Empty, string.Empty, null) { }

    public string Document { get; private set; } = Document;
    public string Fullname { get; private set; } = Fullname;
    public Phone? Phone { get; private set; } = Phone;
    // public Email? Email { get; private set; } = Email;
    // public Address? Address { get; private set; } = Address;
    public Client Update(string fullname, string document, Phone? phone)
    {
        if (!string.IsNullOrEmpty(document)) Document = document;
        if (!string.IsNullOrEmpty(fullname)) Fullname = fullname;
        UpdatePhone(phone);
        return this;
    }

    private void UpdatePhone(Phone? phone)
    {
        if (phone is null) return;

        Phone = new Phone(phone.CountryCode, phone.AreaCode, phone.Number, phone.Type);
    }
}
