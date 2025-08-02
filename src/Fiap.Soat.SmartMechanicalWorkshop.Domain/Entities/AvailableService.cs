namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class AvailableService : Entity
{
    private AvailableService() { }

    public AvailableService(string name, decimal price)
        : this()
    {
        Name = name;
        Price = price;
    }

    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public ICollection<ServiceOrder> ServiceOrders { get; private set; } = [];
    public ICollection<Supply> Supplies { get; private set; } = [];

    public AvailableService Update(string name, decimal? price)
    {
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (price.HasValue) Price = price.Value;
        return this;
    }

    public AvailableService AddSupply(Supply supply)
    {
        Supplies.Add(supply);
        return this;
    }

    public AvailableService AddSupplies(IReadOnlyList<Supply> supplies)
    {
        Supplies.Clear();
        if (!supplies.Any()) return this;
        foreach (var supply in supplies) _ = AddSupply(supply);
        return this;
    }
}
