using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

[ExcludeFromCodeCoverage]
public record UpdateOneSupplyInput(Guid Id, string Name, int? Quantity, decimal? Price);
