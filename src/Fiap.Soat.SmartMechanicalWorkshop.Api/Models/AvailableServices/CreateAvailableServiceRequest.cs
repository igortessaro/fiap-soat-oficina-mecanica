using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Models.AvailableServices;

[ExcludeFromCodeCoverage]
public record CreateAvailableServiceRequest(
    [Required] [MaxLength(100)] string Name,
    [Required] decimal Price,
    IReadOnlyList<ServiceSupplyRequest> Supplies);
