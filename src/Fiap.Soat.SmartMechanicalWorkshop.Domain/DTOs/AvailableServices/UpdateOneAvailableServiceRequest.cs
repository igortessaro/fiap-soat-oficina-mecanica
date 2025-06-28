namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public record UpdateOneAvailableServiceRequest(string Name, decimal? Price, IReadOnlyList<Guid> Supplies);
