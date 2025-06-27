
namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

public class UpdateOneSupplyInput
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
    public Guid Id { get; set; }
}
