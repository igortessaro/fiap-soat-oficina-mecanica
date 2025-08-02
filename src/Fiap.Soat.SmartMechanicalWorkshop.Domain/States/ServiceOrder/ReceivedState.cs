using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;

public sealed class ReceivedState : ServiceOrderState
{
    public override ServiceOrderStatus Status => ServiceOrderStatus.Received;
    public override void ChangeStatus(Entities.ServiceOrder serviceOrder, ServiceOrderStatus status)
    {
        if (status != ServiceOrderStatus.UnderDiagnosis)
        {
            throw new DomainException("Uma ordem de serviço recebida só pode ser alterada para sob diagnóstico.");
        }

        _ = serviceOrder.SetState(new UnderDiagnosisState());
    }
}
