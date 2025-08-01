using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;

public record UpdateOnePersonInput(Guid Id, string Fullname, string Document, EPersonType PersonType, EEmployeeRole? EmployeeRole, string Email, string Password, UpdateOnePhoneInput? Phone, UpdateOneAddressInput? Address);

public record UpdateOnePhoneInput(string AreaCode, string Number)
{
    public static implicit operator UpdateOnePhoneInput?(UpdateOnePhoneRequest? input)
    {
        return input is null ? null : new UpdateOnePhoneInput(input.AreaCode, input.Number);
    }
}

public record UpdateOneAddressInput(string Street, string City, string State, string ZipCode)
{
    public static implicit operator UpdateOneAddressInput?(UpdateOneAddressRequest? input)
    {
        return input is null ? null : new UpdateOneAddressInput(input.Street, input.City, input.State, input.ZipCode);
    }
}
