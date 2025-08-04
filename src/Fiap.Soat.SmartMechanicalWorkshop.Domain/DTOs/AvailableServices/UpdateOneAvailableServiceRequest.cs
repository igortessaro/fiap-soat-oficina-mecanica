using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;

[ExcludeFromCodeCoverage]
public record UpdateOneAvailableServiceRequest(string Name, decimal? Price, IReadOnlyList<ServiceSupplyRequest> Supplies);
