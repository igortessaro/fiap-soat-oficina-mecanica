namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public record UpdateOneAvailableServiceInput
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal? Price { get; init; }
}
