namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record UpdateOneServiceOrderInput(Guid Id, IReadOnlyList<Guid> ServiceIds, string Title, string Description);
