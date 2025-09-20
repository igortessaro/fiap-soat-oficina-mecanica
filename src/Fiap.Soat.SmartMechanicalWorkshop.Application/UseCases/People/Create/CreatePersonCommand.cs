using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Create;

public record CreatePersonCommand(
    string Fullname,
    string Document,
    PersonType PersonType,
    EmployeeRole? EmployeeRole,
    string Email,
    string Password,
    CreatePhoneCommand Phone,
    CreateAddressCommand Address) : IRequest<Response<Person>>;

public record CreatePhoneCommand(string AreaCode, string Number);

public record CreateAddressCommand(string Street, string City, string State, string ZipCode);
