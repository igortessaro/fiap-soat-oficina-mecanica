using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface IClientService
{
    Task<Response<ClientDto>> CreateAsync(CreateClientRequest request, CancellationToken cancellationToken);
    Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<ClientDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<ClientDto>> UpdateAsync(UpdateOneClientInput input, CancellationToken cancellationToken);
    Task<Response<Paginate<ClientDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
}