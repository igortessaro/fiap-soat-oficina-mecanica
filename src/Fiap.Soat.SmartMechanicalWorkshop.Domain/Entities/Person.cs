using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class Person : Entity
{
    private Person() { }

    public string Document { get; private set; } = string.Empty;
    public string Fullname { get; private set; } = string.Empty;
    public EPersonType PersonType { get; private set; }
    public EEmployeeRole? EmployeeRole { get; private set; }
    public Phone Phone { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Guid AddressId { get; private set; }
    public Address Address { get; private set; } = null!;
    public ICollection<Vehicle> Vehicles { get; private set; } = [];

    public Person Update(string fullname, string document, EPersonType personType, EEmployeeRole? employeeRole, string email, Phone phone, Address? address)
    {
        if (!string.IsNullOrEmpty(document)) Document = document;
        if (!string.IsNullOrEmpty(fullname)) Fullname = fullname;
        PersonType = personType;
        EmployeeRole = employeeRole;
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

    public void Validate()
    {
        if(PersonType == EPersonType.Client && EmployeeRole!=null)
        {
            throw new DomainException("Client cannot have an employee role.");
        }
    }
}
