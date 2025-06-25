using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;
using System.Text.RegularExpressions;

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
            if (!IsValidLicensePlate(request.LicensePlate))
            {
                return Result<VehicleDto>.Fail(new FluentResults.Error("Invalid license plate format"), System.Net.HttpStatusCode.BadRequest);
            }

            Vehicle mapperEntity = mapper.Map<Vehicle>(request);
            mapperEntity.LicensePlate = request.LicensePlate.Trim().ToUpper();

            Vehicle createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
            return createdEntity != null
                ? Result<VehicleDto>.Ok(mapper.Map<VehicleDto>(createdEntity), System.Net.HttpStatusCode.Created)
                : Result<VehicleDto>.Fail(new FluentResults.Error("Not Created"));
        }

        private bool IsValidLicensePlate(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                return false;

            licensePlate = licensePlate.Trim().ToUpper();

            var oldPattern = new Regex(@"^[A-Z]{3}-?[0-9]{4}$");
            var newMercosulPattern = new Regex(@"^[A-Z]{3}[0-9][A-Z][0-9]{2}$");

            return oldPattern.IsMatch(licensePlate) || newMercosulPattern.IsMatch(licensePlate);
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            Vehicle foundEntity = await repository.GetByIdAsync(id, cancellationToken);

            if (foundEntity == null)
            {
                return Result.Fail(new FluentResults.Error("Vehicle not found"), System.Net.HttpStatusCode.NotFound);
            }

            await repository.DeleteAsync(foundEntity, cancellationToken);

            return Result.Ok();
        }

        public async Task<Result<Paginate<VehicleDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
        {
            Paginate<Vehicle> response = await repository.GetAllAsync(paginatedRequest, cancellationToken);
            Paginate<VehicleDto> mappedResponse = mapper.Map<Paginate<VehicleDto>>(response);
            return Result<Paginate<VehicleDto>>.Ok(mappedResponse);
        }

        public async Task<Result<VehicleDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
        {
            Vehicle foundEntity = await repository.GetByIdAsync(id, cancellationToken);
            return foundEntity != null
                ? Result<VehicleDto>.Ok(mapper.Map<VehicleDto>(foundEntity))
                : Result<VehicleDto>.Fail(new FluentResults.Error("Vehicle Not Found"), System.Net.HttpStatusCode.NotFound);
        }

        public async Task<Result<VehicleDto>> UpdateAsync(UpdateOneVehicleInput input, CancellationToken cancellationToken)
        {
            Vehicle foundEntity = await repository.GetByIdAsync(input.Id, cancellationToken);

            if (foundEntity == null)
            {
                return Result<VehicleDto>.Fail(new FluentResults.Error("Vehicle not found"), System.Net.HttpStatusCode.NotFound);
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

            Vehicle updatedEntity = await repository.UpdateAsync(foundEntity, cancellationToken);

            return updatedEntity != null
                ? Result<VehicleDto>.Ok(mapper.Map<VehicleDto>(updatedEntity))
                : Result<VehicleDto>.Fail(new FluentResults.Error("Not Updated"));
        }
    }
}
