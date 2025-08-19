using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Models.Person;

[ExcludeFromCodeCoverage]
public record UpdateOnePersonRequest(
    string Fullname,
    string Document,
    PersonType PersonType,
    EmployeeRole? EmployeeRole,
    string Email,
    string Password,
    UpdateOnePhoneRequest? Phone,
    UpdateOneAddressRequest? Address);

[ExcludeFromCodeCoverage]
public record UpdateOnePhoneRequest(string AreaCode, string Number);

[ExcludeFromCodeCoverage]
public record UpdateOneAddressRequest(string Street, string City, string State, string ZipCode);
