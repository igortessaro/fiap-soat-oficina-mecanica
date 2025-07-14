using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public sealed class WaitingApprovalState : ServiceOrderState
{
    public override EServiceOrderStatus Status => EServiceOrderStatus.WaitingApproval;
    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, EServiceOrderStatus status)
    {
        _ = status switch
        {
            EServiceOrderStatus.InProgress => serviceOrder.SetState(new InProgressState()),
            EServiceOrderStatus.Rejected => serviceOrder.SetState(new RejectedState()),
            EServiceOrderStatus.Cancelled => serviceOrder.SetState(new CancelledState()),
            _ => throw new DomainException("Uma ordem de serviço esperando aprovação só pode ser alterada para em progresso, rejeitada ou cancelada.")
        };
    }
}
