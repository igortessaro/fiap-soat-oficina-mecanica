namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

public enum EServiceOrderStatus
{
    Received = 1
        , UnderDiagnosis, WaitingApproval, InProgress, Completed, Delivered, Cancelled, Rejected
}
