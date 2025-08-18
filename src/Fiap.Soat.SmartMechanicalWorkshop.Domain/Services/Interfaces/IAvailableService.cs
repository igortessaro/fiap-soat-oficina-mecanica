using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface IAvailableService
{
    Task<Response<AvailableServiceDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<Paginate<AvailableServiceDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
}
