using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface IPersonService
{
    Task<Response<PersonDto>> CreateAsync(CreatePersonRequest request, CancellationToken cancellationToken);
    Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<PersonDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
    Task<Response<PersonDto>> UpdateAsync(UpdateOnePersonInput input, CancellationToken cancellationToken);
    Task<Response<Paginate<PersonDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    Task<Response<PersonDto>> GetOneByLoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
}
