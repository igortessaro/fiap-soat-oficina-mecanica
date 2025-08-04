using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

[ExcludeFromCodeCoverage]
public record ServiceSupplyInput(Guid SupplyId, int Quantity);
