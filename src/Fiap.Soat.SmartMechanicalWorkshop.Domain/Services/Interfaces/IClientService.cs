using AutoRepairShopManagementSystem.Domains.DTOs;
using AutoRepairShopManagementSystem.Domains.Requests.Clients;
using AutoRepairShopManagementSystem.Shared;
using FluentResults;

namespace AutoRepairShopManagementSystem.Services.Interfaces
{
    public interface IClientService
    {
        Task<Result<ClientDto>> CreateAsync(CreateNewClientRequest request, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<ClientDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<ClientDto>> UpdateAsync(UpdateOneClientInput input, CancellationToken cancellationToken);
        Task<Result<Paginate<ClientDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    }
}
