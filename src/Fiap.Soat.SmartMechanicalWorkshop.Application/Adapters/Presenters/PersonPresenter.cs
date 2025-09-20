using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public static class PersonPresenter
{
    public static PersonDto ToDto(Person entity)
    {
        return new PersonDto(
            entity.Id,
            entity.Fullname,
            entity.Document,
            entity.PersonType,
            entity.EmployeeRole,
            entity.Phone,
            entity.Email,
            entity.Address is null ? null : AddressPresenter.ToDto(entity.Address),
            entity.Vehicles.Select(VehiclePresenter.ToDto).ToList());
    }
}
