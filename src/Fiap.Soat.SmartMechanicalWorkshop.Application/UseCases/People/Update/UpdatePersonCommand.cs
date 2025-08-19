using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
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
    UpdateAddressCommand? Address) : IRequest<Response<PersonDto>>;

public record UpdatePhoneCommand(string AreaCode, string Number)
{
    public static implicit operator UpdatePhoneCommand?(UpdateOnePhoneRequest? input)
    {
        return input is null ? null : new UpdatePhoneCommand(input.AreaCode, input.Number);
    }
}

public record UpdateAddressCommand(string Street, string City, string State, string ZipCode)
{
    public static implicit operator UpdateAddressCommand?(UpdateOneAddressRequest? input)
    {
        return input is null ? null : new UpdateAddressCommand(input.Street, input.City, input.State, input.ZipCode);
    }
}
