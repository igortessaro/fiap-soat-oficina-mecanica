using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentResults;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services
{
    public interface IAvailableService
    {
        Task<Result<AvailableServiceDto>> CreateAsync(CreateAvailableServiceRequest request, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<AvailableServiceDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<AvailableServiceDto>> UpdateAsync(UpdateOneAvailableServiceInput input, CancellationToken cancellationToken);
        Task<Result<Paginate<AvailableServiceDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    }
}