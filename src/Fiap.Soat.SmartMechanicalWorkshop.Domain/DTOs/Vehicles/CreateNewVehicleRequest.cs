using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;

public class CreateNewVehicleRequest
{
    [Required]
    public string LicensePlate { get; set; }
    [Required]
    public DateOnly ManufactureYear { get; set; }
    [Required]
    public string Brand { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public Guid ClientId { get; set; }
}
