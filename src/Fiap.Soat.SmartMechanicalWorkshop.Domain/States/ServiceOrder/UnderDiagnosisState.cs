using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public class UnderDiagnosisState : ServiceOrderState
{
    public override EServiceOrderStatus Status => EServiceOrderStatus.UnderDiagnosis;
    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, EServiceOrderStatus status)
    {
        _ = status switch
        {
            EServiceOrderStatus.WaitingApproval => serviceOrder.SetState(new WaitingApprovalState()),
            EServiceOrderStatus.Cancelled => serviceOrder.SetState(new CancelledState()),
            _ => throw new DomainException("Uma ordem de serviço sob diagnóstico só pode ser alterada para esperando aprovação ou cancelada.")
        };
    }
}
