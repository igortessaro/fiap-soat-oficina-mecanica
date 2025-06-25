using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services
{
    public interface IVehicleService
    {
        Task<Response<VehicleDto>> CreateAsync(CreateNewVehicleRequest request, CancellationToken cancellationToken);

        Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Response<VehicleDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
        Task<Response<VehicleDto>> UpdateAsync(UpdateOneVehicleInput input, CancellationToken cancellationToken);
        Task<Response<Paginate<VehicleDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    }
}
