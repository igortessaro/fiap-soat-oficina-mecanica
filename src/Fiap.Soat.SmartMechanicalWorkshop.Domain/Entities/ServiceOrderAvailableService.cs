namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class ServiceOrderAvailableService : Entity
{
    public Guid AvailableServiceId { get; private set; }
    public Guid ServiceOrderId { get; private set; }

    public AvailableService AvailableService { get; private set; } = null!;
    public ServiceOrder ServiceOrder { get; private set; } = null!;
}
