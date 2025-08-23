using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.ServiceOrders;

[ExcludeFromCodeCoverage]
public record UpdateOneServiceOrderRequest(IReadOnlyList<Guid> ServiceIds, string Title, string Description);
