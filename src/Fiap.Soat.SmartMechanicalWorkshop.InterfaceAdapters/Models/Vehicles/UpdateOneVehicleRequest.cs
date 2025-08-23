using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Vehicles;

[ExcludeFromCodeCoverage]
public record UpdateOneVehicleRequest
{
    public string LicensePlate { get; init; } = string.Empty;
    public int? ManufactureYear { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public Guid? PersonId { get; init; }
}
