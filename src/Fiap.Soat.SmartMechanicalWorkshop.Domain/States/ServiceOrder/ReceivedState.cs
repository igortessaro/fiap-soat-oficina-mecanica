using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public sealed class ReceivedState : ServiceOrderState
{
    public override EServiceOrderStatus Status => EServiceOrderStatus.Received;
    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, EServiceOrderStatus status)
    {
        if (status != EServiceOrderStatus.UnderDiagnosis)
        {
            throw new DomainException("Uma ordem de serviço recebida só pode ser alterada para sob diagnóstico.");
        }

        _ = serviceOrder.SetState(new UnderDiagnosisState());
    }
}
