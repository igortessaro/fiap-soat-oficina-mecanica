using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

[ExcludeFromCodeCoverage]
public record SupplyDto(Guid Id, string Name, int Quantity, decimal Price);
