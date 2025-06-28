namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;

public record UpdateOneVehicleInput(Guid Id, string LicensePlate, int? ManufactureYear, string Brand, string Model, Guid? ClientId);
