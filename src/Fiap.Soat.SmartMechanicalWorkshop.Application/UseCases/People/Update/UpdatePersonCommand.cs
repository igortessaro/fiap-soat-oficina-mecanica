using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Update;

public record UpdatePersonCommand(
    Guid Id,
    string Fullname,
    string Document,
    PersonType PersonType,
    EmployeeRole? EmployeeRole,
    string Email,
    string Password,
    UpdatePhoneCommand? Phone,
    UpdateAddressCommand? Address) : IRequest<Response<Person>>;

public record UpdatePhoneCommand(string AreaCode, string Number);

public record UpdateAddressCommand(string Street, string City, string State, string ZipCode);
