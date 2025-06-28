namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;

public record Supply : Entity
{
    public string Name { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public IReadOnlyList<AvailableService> AvailableServices { get; set; } = [];

    public Supply Update(string name, decimal? price, int? quantity)
    {
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (price.HasValue) Price = price.Value;
        if (quantity.HasValue) Quantity = quantity.Value;
        return this;
    }
}
