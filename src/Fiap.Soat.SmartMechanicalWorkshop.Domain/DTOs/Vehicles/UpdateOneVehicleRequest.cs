namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;

public record UpdateOneVehicleRequest
{
    public string LicensePlate { get; init; } = string.Empty;
    public int? ManufactureYear { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public Guid? ClientId { get; init; }
}
