namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

public record UpdateOneSupplyInput(Guid Id, string Name, int? Quantity, decimal? Price);
