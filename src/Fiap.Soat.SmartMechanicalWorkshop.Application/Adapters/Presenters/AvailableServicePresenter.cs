using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public static class AvailableServicePresenter
{
    public static AvailableServiceDto ToDto(AvailableService entity) => new AvailableServiceDto(entity.Id, entity.Name, entity.Price, entity.AvailableServiceSupplies.Select(SupplyPresenter.ToDto).ToList());
}
