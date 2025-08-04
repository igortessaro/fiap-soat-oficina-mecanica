using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

[ExcludeFromCodeCoverage]
public record CreateServiceOrderRequest(
    [Required] Guid ClientId,
    [Required] Guid VehicleId,
    IReadOnlyList<Guid> ServiceIds,
    [Required] string Title,
    [Required] string Description);
