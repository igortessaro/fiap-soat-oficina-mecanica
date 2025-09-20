using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public sealed class AddressPresenter
{
    public static AddressDto ToDto(Address entity) => new AddressDto(entity.Street, entity.City, entity.State, entity.ZipCode);
}
