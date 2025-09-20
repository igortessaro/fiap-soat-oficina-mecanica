using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public static class SupplyPresenter
{
    public static SupplyDto ToDto(AvailableServiceSupply entity) => new SupplyDto(entity.SupplyId, entity.Supply.Name, entity.Quantity, entity.Supply.Price);
    public static SupplyDto ToDto(Supply entity) => new SupplyDto(entity.Id, entity.Name, entity.Quantity, entity.Price);
}
