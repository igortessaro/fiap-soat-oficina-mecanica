namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class AvailableService : Entity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public ICollection<AvailableServiceSupply> AvailableServiceSupplies { get; private set; } = [];
    public ICollection<ServiceOrderAvailableService> ServiceOrderAvailableServices { get; private set; } = [];
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
        AvailableServiceSupplies.Add(new AvailableServiceSupply(this, supply));
        Supplies.Add(supply);
        return this;
    }

    public AvailableService SetSupplies(List<Supply> supplies)
    {
        AvailableServiceSupplies.Clear();
        if (!supplies.Any()) return this;
        foreach (var supply in supplies) _ = AddSupply(supply);
        return this;
    }
}
