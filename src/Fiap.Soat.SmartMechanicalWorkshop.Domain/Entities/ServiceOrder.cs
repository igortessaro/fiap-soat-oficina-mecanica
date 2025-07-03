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
        Status = ServiceOrderStatus.Received;
    }

    public ServiceOrderStatus Status { get; private set; } = ServiceOrderStatus.Received;
    public Guid ClientId { get; private set; }
    public Guid VehicleId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime? VehicleCheckOutDate { get; private set; }
    public DateTime VehicleCheckInDate { get; private set; }
    public Client Client { get; private set; } = null!;
    public Vehicle Vehicle { get; private set; } = null!;
    public ICollection<ServiceOrderAvailableService> ServiceOrderAvailableServices { get; private set; } = [];

    public ServiceOrder AddAvailableService(AvailableService availableService)
    {
        ServiceOrderAvailableServices.Add(new ServiceOrderAvailableService(availableService, this));
        return this;
    }

    public ServiceOrder Update(string title, string description)
    {
        if (!string.IsNullOrEmpty(title)) Title = title;
        if (!string.IsNullOrEmpty(description)) Description = description;
        return this;
    }

    public ServiceOrder Update(string? title, string? description, ServiceOrderStatus? serviceOrderStatus, DateTime? vehicleCheckInDate, DateTime? vehicleCheckOutDate)
    {
        if (!string.IsNullOrEmpty(title)) Title = title;
        if (!string.IsNullOrEmpty(description)) Description = description;
        if (serviceOrderStatus != null) Status = serviceOrderStatus.Value;
        if (vehicleCheckInDate != null) VehicleCheckInDate = vehicleCheckInDate.Value;
        if (vehicleCheckOutDate != null) VehicleCheckOutDate = vehicleCheckOutDate.Value;
        return this;
    }
}
