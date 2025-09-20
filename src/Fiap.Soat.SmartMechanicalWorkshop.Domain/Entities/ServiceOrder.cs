using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.States.ServiceOrder;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public sealed class ServiceOrder : Entity
{
    private ServiceOrder() { }

    public ServiceOrder(string title, string description, Guid vehicleId, Guid clientId)
        : this()
    {
        Title = title;
        Description = description;
        VehicleId = vehicleId;
        ClientId = clientId;
    }

    private ServiceOrderState _state;

    public ServiceOrderStatus Status { get; private set; }
    public Guid ClientId { get; private set; }
    public Guid VehicleId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Person Client { get; private set; } = null!;
    public Vehicle Vehicle { get; private set; } = null!;
    public ICollection<AvailableService> AvailableServices { get; private set; } = [];
    public ICollection<ServiceOrderEvent> Events { get; private set; } = [];
    public ICollection<Quote> Quotes { get; private set; } = [];

    public ServiceOrder AddAvailableService(AvailableService availableService)
    {
        if (!CanBeUpdated())
        {
            throw new DomainException($"Service Order with status {Status} cannot be updated.");
        }

        AvailableServices.Add(availableService);
        return this;
    }

    private ServiceOrder AddAvailableServices(IReadOnlyList<AvailableService> services)
    {
        AvailableServices.Clear();
        if (!services.Any()) return this;
        foreach (var service in services) _ = AddAvailableService(service);
        return this;
    }

    public ServiceOrder Update(string title, string description, IReadOnlyList<AvailableService> services)
    {
        if (!CanBeUpdated())
        {
            throw new DomainException($"Service Order with status {Status} cannot be updated.");
        }

        if (!string.IsNullOrEmpty(title)) Title = title;
        if (!string.IsNullOrEmpty(description)) Description = description;
        _ = AddAvailableServices(services);
        return this;
    }

    public ServiceOrder SetState(ServiceOrderState state)
    {
        _state = state;
        Status = state.Status;
        return this;
    }

    public ServiceOrder ChangeStatus(ServiceOrderStatus newStatus)
    {
        _state.ChangeStatus(this, newStatus);
        return this;
    }

    public ServiceOrder SyncState()
    {
        _state = Status switch
        {
            ServiceOrderStatus.Received => new ReceivedState(),
            ServiceOrderStatus.UnderDiagnosis => new UnderDiagnosisState(),
            ServiceOrderStatus.WaitingApproval => new WaitingApprovalState(),
            ServiceOrderStatus.InProgress => new InProgressState(),
            ServiceOrderStatus.Completed => new CompletedState(),
            ServiceOrderStatus.Delivered => new DeliveredState(),
            ServiceOrderStatus.Cancelled => new CancelledState(),
            ServiceOrderStatus.Rejected => new RejectedState(),
            _ => throw new InvalidOperationException("Unknown status")
        };

        return this;
    }

    public static ServiceOrderStatus GetNextStatus(QuoteStatus quoteStatus)
    {
        return quoteStatus switch
        {
            QuoteStatus.Pending => ServiceOrderStatus.UnderDiagnosis,
            QuoteStatus.Approved => ServiceOrderStatus.InProgress,
            QuoteStatus.Rejected => ServiceOrderStatus.Rejected,
            _ => throw new DomainException($"Cannot determine next status for quote with status {quoteStatus}.")
        };
    }

    private bool CanBeUpdated() => Status is ServiceOrderStatus.Received or ServiceOrderStatus.UnderDiagnosis;
}
