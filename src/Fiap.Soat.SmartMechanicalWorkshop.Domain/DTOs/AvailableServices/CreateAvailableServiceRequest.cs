using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public record CreateAvailableServiceRequest([Required, MaxLength(100)] string Name, [Required] decimal Price, IReadOnlyList<Guid> SuppliesIds);
