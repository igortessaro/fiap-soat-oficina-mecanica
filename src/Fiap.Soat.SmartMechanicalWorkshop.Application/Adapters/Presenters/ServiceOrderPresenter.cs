using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public static class ServiceOrderPresenter
{
    public static ServiceOrderDto ToDto(ServiceOrder entity)
    {
        return new ServiceOrderDto
        {
            Id = entity.Id,
            Status = entity.Status,
            Title = entity.Title,
            Description = entity.Description,
            ClientId = entity.ClientId,
            VehicleId = entity.VehicleId,
            Client = entity.Client is null ? null : PersonPresenter.ToDto(entity.Client),
            Vehicle = entity.Vehicle is null ? null : VehiclePresenter.ToDto(entity.Vehicle),
            AvailableServices = entity.AvailableServices.Select(AvailableServicePresenter.ToDto).ToList(),
            Events = entity.Events.Select(ServiceOrderEventPresenter.ToDto).ToList(),
            Quotes = entity.Quotes.Select(QuotePresenter.ToDto).ToList()
        };
    }
}
