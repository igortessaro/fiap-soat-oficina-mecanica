namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

public class ServiceOrder : Entity
{
    public string Status { get; private set; } = string.Empty;
    public Guid ClientId { get; private set; }
    public Guid VehicleId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public Client Client { get; private set; } = null!;
    public Vehicle Vehicle { get; private set; } = null!;
    // public ICollection<AvailableService> AvailableServices { get; private set; } = [];
    public ICollection<ServiceOrderAvailableService> ServiceOrderAvailableServices { get; private set; } = [];
}
