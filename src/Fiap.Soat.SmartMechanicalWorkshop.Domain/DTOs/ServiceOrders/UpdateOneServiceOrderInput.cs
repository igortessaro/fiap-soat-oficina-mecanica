using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record UpdateOneServiceOrderInput(Guid Id, IReadOnlyList<Guid>? ServiceIds, string? Title, string? Description,
    DateTime? VehicleCheckOutDate,
    DateTime? VehicleCheckInDate,
    ServiceOrderStatus? ServiceOrderStatus)
{
    public UpdateOneServiceOrderInput() : this(default, null, null, null, null, null, null)
    {
    }
}
