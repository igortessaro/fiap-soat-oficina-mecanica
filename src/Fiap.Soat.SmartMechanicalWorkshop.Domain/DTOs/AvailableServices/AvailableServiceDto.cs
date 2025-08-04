using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

[ExcludeFromCodeCoverage]
public record AvailableServiceDto(Guid Id, string Name, decimal Price, IReadOnlyList<SupplyDto> Supplies);
