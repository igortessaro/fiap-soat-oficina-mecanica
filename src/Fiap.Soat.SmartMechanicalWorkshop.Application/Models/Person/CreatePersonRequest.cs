using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Person;

[ExcludeFromCodeCoverage]
public record CreatePersonRequest(
    [Required][MaxLength(255)] string Fullname,
    [Required][MaxLength(100)] string Document,
    [Required] PersonType PersonType,
    EmployeeRole? EmployeeRole,
    [Required][MaxLength(255)] string Email,
    [Required][MaxLength(255)] string Password,
    CreatePhoneRequest Phone,
    CreateAddressRequest Address);

[ExcludeFromCodeCoverage]
public record CreatePhoneRequest(
    [Required][MaxLength(5)] string AreaCode,
    [Required][MaxLength(15)] string Number
);

[ExcludeFromCodeCoverage]
public record CreateAddressRequest(
    [Required][MaxLength(100)] string Street,
    [Required][MaxLength(60)] string City,
    [Required][MaxLength(30)] string State,
    [Required][MaxLength(15)] string ZipCode
);
