namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;

public class UpdateOneVehicleInput
{
    public Guid Id { get; set; }
    public string? LicensePlate { get; set; }
    public DateOnly? ManufactureYear { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public Guid? ClientId { get; set; }
}