using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface IServiceOrderService
{
    Task<Response<ServiceOrderDto>> CreateAsync(CreateServiceOrderRequest request, CancellationToken cancellationToken);
    Task<Response> SendForApprovalAsync(SendServiceOrderApprovalRequest request, CancellationToken cancellationToken);
    Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<ServiceOrderDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<ServiceOrderDto>> UpdateAsync(UpdateOneServiceOrderInput input, CancellationToken cancellationToken);
    Task<Response<Paginate<ServiceOrderDto>>> GetAllAsync(Guid? personId, PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    Task<Response<ServiceOrderDto>> PatchAsync(UpdateOneServiceOrderInput input, CancellationToken cancellationToken);
    Task<Response<ServiceOrderDto>> GetOneByLoginAsync(GetOnePersonByLoginInput getOnePersonByLoginInput, CancellationToken cancellationToken);
    Task<Response<TimeSpan>> GetAverageExecutionTime(CancellationToken cancellationToken);
}
