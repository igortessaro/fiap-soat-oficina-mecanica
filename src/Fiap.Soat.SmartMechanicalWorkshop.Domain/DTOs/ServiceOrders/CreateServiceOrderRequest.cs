using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record CreateServiceOrderRequest(
    [Required] Guid ClientId,
    [Required] Guid VehicleId,
    IReadOnlyList<Guid> ServiceIds,
    [Required] string Title,
    [Required] string Description
    );
