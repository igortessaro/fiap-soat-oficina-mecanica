using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Models.AvailableServices;

[ExcludeFromCodeCoverage]
public record UpdateOneAvailableServiceRequest(string Name, decimal? Price, IReadOnlyList<ServiceSupplyRequest> Supplies);
