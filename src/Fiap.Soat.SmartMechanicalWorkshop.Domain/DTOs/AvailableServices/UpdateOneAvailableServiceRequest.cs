namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public record UpdateOneAvailableServiceRequest
{
    public string Name { get; init; } = string.Empty;
    public decimal? Price { get; init; }
}
