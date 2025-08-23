using Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Models.Person;
using Microsoft.AspNetCore.Mvc;

namespace Fiap.Soat.SmartMechanicalWorkshop.InterfaceAdapters.Controllers.Interfaces;

public interface IPeopleController
{
    Task<IActionResult> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    Task<IActionResult> CreateAsync(CreatePersonRequest request, CancellationToken cancellationToken);
    Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<IActionResult> UpdateAsync(Guid id, UpdateOnePersonRequest request, CancellationToken cancellationToken);
}
