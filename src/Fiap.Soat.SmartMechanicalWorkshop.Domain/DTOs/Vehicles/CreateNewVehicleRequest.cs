using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;

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
