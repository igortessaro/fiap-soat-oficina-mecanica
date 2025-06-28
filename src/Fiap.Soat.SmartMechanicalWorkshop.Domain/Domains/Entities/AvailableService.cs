namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;

public record AvailableService : Entity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public IReadOnlyList<Supply> Supplies { get; private set; } = [];
    public IReadOnlyList<ServiceOrder> ServiceOrders { get; set; } = [];

    public AvailableService Update(string name, decimal? price)
    {
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (price.HasValue) Price = price.Value;
        return this;
    }
}
