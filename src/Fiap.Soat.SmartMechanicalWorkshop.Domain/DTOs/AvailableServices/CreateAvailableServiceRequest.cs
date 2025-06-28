namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public record CreateAvailableServiceRequest
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
}
