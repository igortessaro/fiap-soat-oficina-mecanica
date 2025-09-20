using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public sealed class CompletedState : ServiceOrderState
{
    public override ServiceOrderStatus Status => ServiceOrderStatus.Completed;

    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status)
    {
        if (status != ServiceOrderStatus.Delivered)
        {
            throw new DomainException("Uma ordem de serviço concluída só pode ser alterada para entregue.");
        }

        _ = serviceOrder.SetState(new DeliveredState());
    }
}
