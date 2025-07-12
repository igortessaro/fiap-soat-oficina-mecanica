using Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class ServiceOrder : Entity
{
    private ServiceOrder()
    {
        SyncState();
    }

    public ServiceOrder(string title, string description, Guid vehicleId, Guid clientId)
        :this()
    {
        Title = title;
        Description = description;
        VehicleId = vehicleId;
        ClientId = clientId;
    }

    private ServiceOrderState _state;

    public EServiceOrderStatus Status { get; private set; } = EServiceOrderStatus.Received;
    public Guid ClientId { get; private set; }
    public Guid VehicleId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Person Client { get; private set; } = null!;
    public Vehicle Vehicle { get; private set; } = null!;
    public ICollection<AvailableService> AvailableServices { get; private set; } = [];

    public ServiceOrder AddAvailableService(AvailableService availableService)
    {
        AvailableServices.Add(availableService);
        return this;
    }

    public ServiceOrder Update(string title, string description)
    {
        if (!string.IsNullOrEmpty(title)) Title = title;
        if (!string.IsNullOrEmpty(description)) Description = description;
        return this;
    }

    public ServiceOrder Update(string? title, string? description, EServiceOrderStatus? serviceOrderStatus)
    {
        if (!string.IsNullOrEmpty(title)) Title = title;
        if (!string.IsNullOrEmpty(description)) Description = description;
        if (serviceOrderStatus != null) Status = serviceOrderStatus.Value;
        return this;
    }

    public ServiceOrder SetState(ServiceOrderState state)
    {
        _state = state;
        Status = state.Status;
        return this;
    }

    public ServiceOrder ChangeStatus(EServiceOrderStatus newStatus)
    {
        _state.ChangeStatus(this, newStatus);
        return this;
    }

    public void SyncState()
    {
        _state = Status switch
        {
            EServiceOrderStatus.Received => new ReceivedState(),
            EServiceOrderStatus.UnderDiagnosis => new UnderDiagnosisState(),
            EServiceOrderStatus.WaitingApproval => new WaitingApprovalState(),
            EServiceOrderStatus.InProgress => new InProgressState(),
            EServiceOrderStatus.Completed => new CompletedState(),
            EServiceOrderStatus.Delivered => new DeliveredState(),
            EServiceOrderStatus.Cancelled => new CancelledState(),
            EServiceOrderStatus.Rejected => new RejectedState(),
            _ => throw new InvalidOperationException("Unknown status")
        };
    }
}
