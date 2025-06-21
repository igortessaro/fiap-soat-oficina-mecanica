using AutoMapper;
using AutoRepairShopManagementSystem.Domains.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;
using FluentResults;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services
{
    public class VehicleService(VehicleRepository repository, IMapper mapper) : IVehicleService
    {
        public async Task<Result<VehicleDto>> CreateAsync(CreateNewVehicleRequest request, CancellationToken cancellationToken)
        {
            //var foundClient = await clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
            //if (foundClient == null)
            //{
            //    return Result.Fail(new Error("Client not found"));
            //}

            Vehicle mapperEntity = mapper.Map<Vehicle>(request);
            Vehicle? createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
            return createdEntity != null
                ? Result.Ok(mapper.Map<VehicleDto>(createdEntity))
                : Result.Fail(new Error("Not Created"));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            Vehicle foundEntity = await repository.GetByIdAsync(id, cancellationToken);

            if (foundEntity == null)
            {
                return Result.Fail(new Error("Vehicle not found"));
            }

            await repository.DeleteAsync(foundEntity, cancellationToken);

            return Result.Ok();
        }

        public async Task<Result<Paginate<VehicleDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
        {
            Paginate<Vehicle> response = await repository.GetAllAsync(paginatedRequest, cancellationToken);
            Paginate<VehicleDto> mappedResponse = mapper.Map<Paginate<VehicleDto>>(response);
            return Result.Ok(mappedResponse);
        }

        public async Task<Result<VehicleDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
        {
            return await repository.GetByIdAsync(id, cancellationToken)
              .ContinueWith(task =>
              {
                  Vehicle? foundEntity = task.Result;
                  return foundEntity != null
                      ? Result.Ok<VehicleDto>(mapper.Map<VehicleDto>(foundEntity))
                      : Result.Fail(new Error("Vehicle Not Found"));
              }, cancellationToken);
        }

        public async Task<Result<VehicleDto>> UpdateAsync(UpdateOneVehicleInput input, CancellationToken cancellationToken)
        {
            Vehicle foundEntity = await repository.GetByIdAsync(input.Id, cancellationToken);

            if (foundEntity == null)
            {
                return Result.Fail(new Error("Vehicle not found"));
            }

            if (input.ManufactureYear != null)
            {
                foundEntity.ManufactureYear = (DateOnly)input.ManufactureYear;
            }

            if (input.LicensePlate != null)
            {
                foundEntity.LicensePlate = input.LicensePlate;
            }

            if (input.Brand != null)
            {
                foundEntity.Brand = input.Brand;
            }

            if (input.Model != null)
            {
                foundEntity.Model = input.Model;
            }

            //if (input.ClientId != null)
            //{
            //    Task<Client> foundClient = clientRepository.GetByIdAsync((Guid)input.ClientId, cancellationToken);

            //    if (foundClient == null)
            //    {
            //        return Result.Fail(new Error("Client not found"));
            //    }
            //    foundEntity.ClientId = (Guid)input.ClientId;
            //}

            Vehicle? updatedEntity = await repository.UpdateAsync(foundEntity, cancellationToken);

            return updatedEntity != null
                ? Result.Ok(mapper.Map<VehicleDto>(updatedEntity))
                : Result.Fail(new Error("Not Updated"));
        }
    }
}
