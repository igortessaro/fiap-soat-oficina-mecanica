using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public sealed class ServiceOrderEventPresenter
{
    public static ServiceOrderEventDto ToDto(ServiceOrderEvent entity) =>
        new ServiceOrderEventDto(entity.Id, entity.Status, entity.CreatedAt);
}
