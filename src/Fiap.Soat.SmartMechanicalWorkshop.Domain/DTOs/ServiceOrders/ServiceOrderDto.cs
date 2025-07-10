using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record ServiceOrderDto
{
    public Guid Id { get; init; }
    public EServiceOrderStatus Status { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid ClientId { get; init; }
    public Guid VehicleId { get; init; }
    public PersonDto Person { get; init; } = null!;
    public VehicleDto Vehicle { get; init; } = null!;
    public ICollection<AvailableServiceDto> AvailableServices { get; init; } = [];

}
