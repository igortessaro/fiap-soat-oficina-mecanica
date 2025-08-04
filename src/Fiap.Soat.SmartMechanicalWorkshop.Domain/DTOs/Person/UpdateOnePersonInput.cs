using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;

[ExcludeFromCodeCoverage]
public record UpdateOnePersonInput(
    Guid Id,
    string Fullname,
    string Document,
    PersonType PersonType,
    EmployeeRole? EmployeeRole,
    string Email,
    string Password,
    UpdateOnePhoneInput? Phone,
    UpdateOneAddressInput? Address);

[ExcludeFromCodeCoverage]
public record UpdateOnePhoneInput(string AreaCode, string Number)
{
    public static implicit operator UpdateOnePhoneInput?(UpdateOnePhoneRequest? input)
    {
        return input is null ? null : new UpdateOnePhoneInput(input.AreaCode, input.Number);
    }
}

[ExcludeFromCodeCoverage]
public record UpdateOneAddressInput(string Street, string City, string State, string ZipCode)
{
    public static implicit operator UpdateOneAddressInput?(UpdateOneAddressRequest? input)
    {
        return input is null ? null : new UpdateOneAddressInput(input.Street, input.City, input.State, input.ZipCode);
    }
}
