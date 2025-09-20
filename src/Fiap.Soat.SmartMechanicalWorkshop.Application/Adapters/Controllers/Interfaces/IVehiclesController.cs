using Fiap.Soat.SmartMechanicalWorkshop.Application.Models.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Controllers.Interfaces;

public interface IVehiclesController
{
    Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    Task<IActionResult> CreateAsync(CreateNewVehicleRequest request, CancellationToken cancellationToken);
    Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> UpdateAsync(Guid id, UpdateOneVehicleRequest request, CancellationToken cancellationToken);
}
