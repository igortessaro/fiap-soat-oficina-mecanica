using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public static class VehiclePresenter
{
    public static VehicleDto ToDto(Vehicle entity) => new VehicleDto(entity.Id, entity.LicensePlate, entity.ManufactureYear, entity.Brand, entity.Model, entity.PersonId);
}
