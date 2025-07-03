using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record UpdateOneServiceOrderInput
{
    public Guid Id { get; init; }
    public IReadOnlyList<Guid>? ServiceIds { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? VehicleCheckOutDate { get; init; }
    public DateTime? VehicleCheckInDate { get; init; }
    public ServiceOrderStatus? ServiceOrderStatus { get; init; }

    public UpdateOneServiceOrderInput() { }
    public UpdateOneServiceOrderInput(
        Guid id,
        IReadOnlyList<Guid>? serviceIds,
        string? title,
        string? description,
        DateTime? vehicleCheckInDate,
        DateTime? vehicleCheckOutDate,
        ServiceOrderStatus? serviceOrderStatus)
    {
        Id = id;
        ServiceIds = serviceIds;
        Title = title;
        Description = description;
        VehicleCheckInDate = vehicleCheckInDate;
        VehicleCheckOutDate = vehicleCheckOutDate;
        ServiceOrderStatus = serviceOrderStatus;
    }

    public UpdateOneServiceOrderInput(Guid id, ServiceOrderStatus status)
    {
        Id = id;
        ServiceOrderStatus = status;
    }
}
