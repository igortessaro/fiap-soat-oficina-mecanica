using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;

public interface IAvailableServicesController
{
    Task<ActionResult> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<ActionResult> GetAllAsync(PaginatedRequest paginatedQuery, CancellationToken cancellationToken);
    Task<IActionResult> CreateAsync(CreateAvailableServiceRequest request, CancellationToken cancellationToken);
    Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> UpdateAsync(Guid id, UpdateOneAvailableServiceRequest request, CancellationToken cancellationToken);
}
