namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;

public record VehicleDto(Guid Id, string LicensePlate, int ManufactureYear, string Brand, string Model, Guid ClientId);
