namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public class UpdateOneAvailableServiceInput
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}