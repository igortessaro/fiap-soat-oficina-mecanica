using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public sealed class CancelledState : ServiceOrderState
{
    public override EServiceOrderStatus Status => EServiceOrderStatus.Cancelled;
    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, EServiceOrderStatus status)
    {
        if (status != EServiceOrderStatus.Delivered)
        {
            throw new DomainException("Uma ordem de serviço cancelada só pode ser alterada para entregue.");
        }

        _ = serviceOrder.SetState(new DeliveredState());
    }
}
