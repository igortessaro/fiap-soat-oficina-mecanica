using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Create;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Supplies;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers.Interfaces;

public interface ISuppliesController
{
    Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> GetAllAsync(PaginatedRequest paginatedQuery, CancellationToken cancellationToken);
    Task<IActionResult> CreateAsync(CreateSupplyCommand command, CancellationToken cancellationToken);
    Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> UpdateAsync(Guid id, UpdateOneSupplyRequest request, CancellationToken cancellationToken);
}
