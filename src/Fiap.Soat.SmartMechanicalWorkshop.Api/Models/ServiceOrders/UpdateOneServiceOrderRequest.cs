using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Models.ServiceOrders;

[ExcludeFromCodeCoverage]
public record UpdateOneServiceOrderRequest(IReadOnlyList<Guid> ServiceIds, string Title, string Description);
