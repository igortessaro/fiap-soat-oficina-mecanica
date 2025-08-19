using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
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
    CreateAddressCommand Address) : IRequest<Response<PersonDto>>
{

    public static implicit operator CreatePersonCommand(CreatePersonRequest request)
    {
        return new CreatePersonCommand(request.Fullname, request.Document, request.PersonType, request.EmployeeRole, request.Email, request.Password, request.Phone, request.Address);
    }
}

public record CreatePhoneCommand(string AreaCode, string Number)
{
    public static implicit operator CreatePhoneCommand(CreatePhoneRequest? input)
    {
        return input is null ? new CreatePhoneCommand(string.Empty, string.Empty) : new CreatePhoneCommand(input.AreaCode, input.Number);
    }
}

public record CreateAddressCommand(string Street, string City, string State, string ZipCode)
{
    public static implicit operator CreateAddressCommand(CreateAddressRequest? input)
    {
        return input is null ?
            new CreateAddressCommand(string.Empty, string.Empty, string.Empty, string.Empty) :
            new CreateAddressCommand(input.Street, input.City, input.State, input.ZipCode);
    }
}
