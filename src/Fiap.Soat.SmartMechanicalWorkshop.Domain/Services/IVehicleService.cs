using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentResults;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services
{
    public interface IVehicleService
    {
        Task<Result<VehicleDto>> CreateAsync(CreateNewVehicleRequest request, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<VehicleDto>> GetOneAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<VehicleDto>> UpdateAsync(UpdateOneVehicleInput input, CancellationToken cancellationToken);
        Task<Result<Paginate<VehicleDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken);
    }
}
