using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record UpdateOneServiceOrderRequest(IReadOnlyList<Guid> ServiceIds, string Title, string Description);
