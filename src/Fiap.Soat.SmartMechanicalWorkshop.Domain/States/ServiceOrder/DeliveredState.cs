using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public sealed class DeliveredState : ServiceOrderState
{
    public override EServiceOrderStatus Status => EServiceOrderStatus.Delivered;
    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, EServiceOrderStatus status)
    {
        if (status != Status)
        {
            throw new DomainException("Uma ordem de serviço entregue não pode ser alterada para outro status.");
        }
    }
}
