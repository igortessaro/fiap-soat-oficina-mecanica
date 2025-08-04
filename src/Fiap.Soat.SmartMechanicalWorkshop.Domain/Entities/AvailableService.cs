using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

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
    public ICollection<AvailableServiceSupply> AvailableServiceSupplies { get; private set; } = [];

    public AvailableService Update(string name, decimal? price)
    {
        if (!string.IsNullOrEmpty(name)) Name = name;
        if (price.HasValue) Price = price.Value;
        return this;
    }

    public AvailableService AddSupply(Guid supplyId, int quantity)
    {
        AvailableServiceSupplies.Add(new AvailableServiceSupply(Id, supplyId, quantity));
        return this;
    }

    public AvailableService AddSupplies(IReadOnlyList<ServiceSupplyDto> supplies)
    {
        AvailableServiceSupplies.Clear();
        if (!supplies.Any()) return this;
        foreach (var supply in supplies) _ = AddSupply(supply.SupplyId, supply.Quantity);
        return this;
    }
}
