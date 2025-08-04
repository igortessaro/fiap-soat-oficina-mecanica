using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record UpdateOneServiceOrderInput(Guid Id, string Title, string Description, IReadOnlyList<Guid> ServiceIds);
