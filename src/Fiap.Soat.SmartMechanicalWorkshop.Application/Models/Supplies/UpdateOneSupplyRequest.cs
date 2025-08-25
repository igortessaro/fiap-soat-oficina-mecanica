using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Supplies;

[ExcludeFromCodeCoverage]
public record UpdateOneSupplyRequest
{
    public string Name { get; init; } = string.Empty;
    public int? Quantity { get; init; }
    public decimal? Price { get; init; }
}
