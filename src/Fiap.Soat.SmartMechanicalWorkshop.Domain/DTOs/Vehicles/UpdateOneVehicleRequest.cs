namespace Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles
{
    public class UpdateOneVehicleRequest
    {
        public string? LicensePlate { get; set; }
        public DateOnly? ManufactureYear { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public Guid? ClientId { get; set; }
    }
}
