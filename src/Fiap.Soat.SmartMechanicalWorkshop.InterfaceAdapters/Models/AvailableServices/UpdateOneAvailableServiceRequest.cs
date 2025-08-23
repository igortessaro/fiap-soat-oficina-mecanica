using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.AvailableServices;

[ExcludeFromCodeCoverage]
public record UpdateOneAvailableServiceRequest(string Name, decimal? Price, IReadOnlyList<ServiceSupplyRequest> Supplies);
