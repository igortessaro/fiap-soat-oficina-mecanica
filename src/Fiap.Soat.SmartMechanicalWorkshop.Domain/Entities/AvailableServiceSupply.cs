namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class AvailableServiceSupply : Entity
{
    private AvailableServiceSupply() { }

    public AvailableServiceSupply(AvailableService availableService, Supply supply) : this()
    {
        AvailableService = availableService;
        AvailableServiceId = availableService.Id;
        SupplyId = supply.Id;
        Supply = supply;
    }

    public Guid AvailableServiceId { get; private set; }
    public Guid SupplyId { get; private set; }

    public AvailableService AvailableService { get; private set; } = null!;
    public Supply Supply { get; private set; } = null!;
}
