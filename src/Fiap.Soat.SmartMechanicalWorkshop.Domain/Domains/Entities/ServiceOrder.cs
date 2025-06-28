namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;

public record ServiceOrder : Entity
{
    public string Status { get; private set; } = string.Empty;
    public Guid ClientId { get; private set; }
    public Guid VehicleId { get; private set; }
    public Client Client { get; private set; } = null!;
    public Vehicle Vehicle { get; private set; } = null!;
    public IReadOnlyList<AvailableService> AvailableServices { get; private set; } = [];
}
