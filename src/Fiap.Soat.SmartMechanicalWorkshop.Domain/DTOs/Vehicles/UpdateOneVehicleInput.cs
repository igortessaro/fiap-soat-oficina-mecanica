using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;

[ExcludeFromCodeCoverage]
public record UpdateOneVehicleInput(Guid Id, string LicensePlate, int? ManufactureYear, string Brand, string Model, Guid? PersonId);
