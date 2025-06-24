using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentResults;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services
{
    public interface ISupplyService
    {
        Task<Result<SupplyDto>> CreateAsync(CreateNewSupplyRequest request, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<SupplyDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<SupplyDto>> UpdateAsync(UpdateOneSupplyInput input, CancellationToken cancellationToken);
        Task<Result<Paginate<SupplyDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);

    }
}
