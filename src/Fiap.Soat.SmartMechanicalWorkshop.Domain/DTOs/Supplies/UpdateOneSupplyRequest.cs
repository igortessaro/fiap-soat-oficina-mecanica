namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

public record UpdateOneSupplyRequest
{
    public string Name { get; init; } = string.Empty;
    public int? Quantity { get; init; }
    public decimal? Price { get; init; }
}
