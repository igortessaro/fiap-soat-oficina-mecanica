namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

public class UpdateOneSupplyRequest
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
}