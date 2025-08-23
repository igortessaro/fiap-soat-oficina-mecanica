using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers.Interfaces;

public interface IVehiclesController
{
    Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    Task<IActionResult> CreateAsync(CreateNewVehicleRequest request, CancellationToken cancellationToken);
    Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> UpdateAsync(Guid id, UpdateOneVehicleRequest request, CancellationToken cancellationToken);
}
