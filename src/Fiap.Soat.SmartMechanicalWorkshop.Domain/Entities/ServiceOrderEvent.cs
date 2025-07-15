using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public sealed class ServiceOrderEvent : Entity
{
    private ServiceOrderEvent() { }

    public ServiceOrderEvent(Guid serviceOrderId, EServiceOrderStatus status)
    {
        ServiceOrderId = serviceOrderId;
        Status = status;
    }

    public EServiceOrderStatus Status { get; private set; }
    public Guid ServiceOrderId { get; private set; }

    public ServiceOrder ServiceOrder { get; private set; } = null!;
}
