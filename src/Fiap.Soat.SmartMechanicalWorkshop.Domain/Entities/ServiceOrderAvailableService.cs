namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class ServiceOrderAvailableService : Entity
{
    private ServiceOrderAvailableService() { }

    public ServiceOrderAvailableService(AvailableService availableService, ServiceOrder serviceOrder) : this()
    {
        AvailableService = availableService;
        AvailableServiceId = availableService.Id;
        ServiceOrderId = serviceOrder.Id;
        ServiceOrder = serviceOrder;
    }

    public Guid AvailableServiceId { get; private set; }
    public Guid ServiceOrderId { get; private set; }

    public AvailableService AvailableService { get; private set; } = null!;
    public ServiceOrder ServiceOrder { get; private set; } = null!;
}
