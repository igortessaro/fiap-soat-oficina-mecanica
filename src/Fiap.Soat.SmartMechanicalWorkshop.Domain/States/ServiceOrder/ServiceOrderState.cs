using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public abstract class ServiceOrderState
{
    public abstract ServiceOrderStatus Status { get; }
    public abstract void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status);
}
