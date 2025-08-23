using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.AvailableServices;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers.Interfaces;

public interface IAvailableServicesController
{
    Task<ActionResult> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<ActionResult> GetAllAsync(PaginatedRequest paginatedQuery, CancellationToken cancellationToken);
    Task<IActionResult> CreateAsync(CreateAvailableServiceRequest request, CancellationToken cancellationToken);
    Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> UpdateAsync(Guid id, UpdateOneAvailableServiceRequest request, CancellationToken cancellationToken);
}
