using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface ISupplyService
{
    Task<Response<SupplyDto>> CreateAsync(CreateNewSupplyRequest request, CancellationToken cancellationToken);

    Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<SupplyDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<SupplyDto>> UpdateAsync(UpdateOneSupplyInput input, CancellationToken cancellationToken);
    Task<Response<Paginate<SupplyDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);

}
