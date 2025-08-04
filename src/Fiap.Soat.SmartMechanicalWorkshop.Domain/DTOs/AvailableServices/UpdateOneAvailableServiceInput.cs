using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

[ExcludeFromCodeCoverage]
public record UpdateOneAvailableServiceInput(Guid Id, string Name, decimal? Price, IReadOnlyList<ServiceSupplyInput> Supplies);
