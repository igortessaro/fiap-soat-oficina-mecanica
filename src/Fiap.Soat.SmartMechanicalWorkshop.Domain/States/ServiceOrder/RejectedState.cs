using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public sealed class RejectedState : ServiceOrderState
{
    public override EServiceOrderStatus Status => EServiceOrderStatus.Rejected;
    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, EServiceOrderStatus status)
    {
        if (status != EServiceOrderStatus.WaitingApproval)
        {
            throw new DomainException("Uma ordem de serviço rejeitada só pode ser alterada para agurdando aprovação.");
        }

        _ = serviceOrder.SetState(new WaitingApprovalState());
    }
}
