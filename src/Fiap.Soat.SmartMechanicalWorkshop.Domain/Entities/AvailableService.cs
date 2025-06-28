namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class AvailableService : Entity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public ICollection<AvailableServiceSupply> AvailableServiceSupplies { get; private set; } = [];
    public ICollection<ServiceOrderAvailableService> ServiceOrderAvailableServices { get; private set; } = [];

    public AvailableService Update(string name, decimal? price)
    {
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (price.HasValue) Price = price.Value;
        return this;
    }

    public AvailableService AddSupply(Supply supply)
    {
        AvailableServiceSupplies.Add(new AvailableServiceSupply(this, supply));
        return this;
    }

    public AvailableService SetSupplies(List<Supply> supplies)
    {
        AvailableServiceSupplies.Clear();
        if (!supplies.Any()) return this;
        foreach (var supply in supplies) _ = AddSupply(supply);
        // var toAdd = supplies.Where(s => AvailableServiceSupplies.All(ass => ass.SupplyId != s.Id)).ToList();
        // var toRemove = AvailableServiceSupplies.Where(ass => supplies.All(s => s.Id != ass.SupplyId)).ToList();
        // foreach (var supply in toAdd) AddSupply(supply);
        // foreach (var supply in toRemove) AvailableServiceSupplies.Remove(supply);
        return this;
    }
}
