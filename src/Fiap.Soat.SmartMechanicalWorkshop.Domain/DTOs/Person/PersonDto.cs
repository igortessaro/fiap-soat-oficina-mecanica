using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;

public record PersonDto(
    Guid Id,
    string Fullname,
    string Document,
    PersonType PersonType,
    EmployeeRole? EmployeeRole,
    string Phone,
    string Email,
    AddressDto Address,
    IReadOnlyList<VehicleDto> Vehicles);

public record PhoneDto(string AreaCode, string Number);

public record EmailDto(string Address);

public record AddressDto(string Street, string City, string State, string ZipCode);
