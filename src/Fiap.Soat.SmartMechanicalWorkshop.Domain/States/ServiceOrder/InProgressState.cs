using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public sealed class InProgressState : ServiceOrderState
{
    public override EServiceOrderStatus Status => EServiceOrderStatus.InProgress;
    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, EServiceOrderStatus status)
    {
        if (status != EServiceOrderStatus.Completed)
        {
            throw new DomainException("Uma ordem de serviço em progresso só pode ser alterada para concluída.");
        }

        _ = serviceOrder.SetState(new CompletedState());
    }
}
