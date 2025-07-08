namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public record UpdateOneAvailableServiceInput(Guid Id, string Name, decimal? Price, IReadOnlyList<Guid>? SuppliesIds);
