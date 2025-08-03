namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record UpdateOneServiceOrderInput(Guid Id, string Title, string Description, IReadOnlyList<Guid> ServiceIds);
