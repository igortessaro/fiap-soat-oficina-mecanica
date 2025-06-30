using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface IAvailableService
{
    Task<Response<AvailableServiceDto>> CreateAsync(CreateAvailableServiceRequest request, CancellationToken cancellationToken);
    Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<AvailableServiceDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<AvailableServiceDto>> UpdateAsync(UpdateOneAvailableServiceInput input, CancellationToken cancellationToken);
    Task<Response<Paginate<AvailableServiceDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
}
