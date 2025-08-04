using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

public record AvailableServiceDto(Guid Id, string Name, decimal Price, IReadOnlyList<SupplyDto> Supplies);
