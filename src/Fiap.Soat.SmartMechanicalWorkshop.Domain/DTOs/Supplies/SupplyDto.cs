namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

public class SupplyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
