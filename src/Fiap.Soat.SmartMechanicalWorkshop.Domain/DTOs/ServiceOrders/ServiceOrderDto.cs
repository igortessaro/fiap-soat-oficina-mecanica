using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;

public record ServiceOrderDto
{
    public Guid Id { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid ClientId { get; init; }
    public Guid VehicleId { get; init; }
    public DateTime VehicleCheckOutDate { get; private set; }
    public DateTime VehicleCheckInDate { get; private set; }
    public ClientDto Client { get; init; } = null!;
    public VehicleDto Vehicle { get; init; } = null!;
    public IReadOnlyList<AvailableServiceDto> AvailableServices { get; init; } = [];
}
