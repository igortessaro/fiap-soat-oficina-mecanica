using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class ServiceOrder : Entity
{
    private ServiceOrder() { }

    public ServiceOrder(string title, string description, Guid vehicleId, Guid clientId)
    {
        Title = title;
        Description = description;
        VehicleId = vehicleId;
        ClientId = clientId;
        Status = EServiceOrderStatus.Received;
    }

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
}
