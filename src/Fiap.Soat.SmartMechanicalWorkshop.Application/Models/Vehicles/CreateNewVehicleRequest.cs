using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Vehicles;

[ExcludeFromCodeCoverage]
public record CreateNewVehicleRequest
{
    [Required]
    public string LicensePlate { get; init; } = string.Empty;
    [Required]
    public int ManufactureYear { get; init; }
    [Required]
    public string Brand { get; init; } = string.Empty;
    [Required]
    public string Model { get; init; } = string.Empty;
    [Required]
    public Guid PersonId { get; init; }
}
