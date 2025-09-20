namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public sealed class AvailableServiceSupply
{
    private AvailableServiceSupply() { }

    public AvailableServiceSupply(Guid availableServiceId, Guid supplyId, int quantity)
        : this()
    {
        AvailableServiceId = availableServiceId;
        SupplyId = supplyId;
        Quantity = quantity;
    }

    public Guid AvailableServiceId { get; private set; }
    public Guid SupplyId { get; private set; }
    public int Quantity { get; private set; }

    public AvailableService AvailableService { get; private set; } = null!;
    public Supply Supply { get; private set; } = null!;
}
