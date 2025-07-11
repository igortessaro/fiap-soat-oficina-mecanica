using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record UpdateOneServiceOrderRequest(IReadOnlyList<Guid>? ServiceIds, string? Title, string? Description,
    EServiceOrderStatus? ServiceOrderStatus);
