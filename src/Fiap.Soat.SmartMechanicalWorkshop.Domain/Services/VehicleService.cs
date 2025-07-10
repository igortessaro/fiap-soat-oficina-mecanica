using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Text.RegularExpressions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public class VehicleService(IVehicleRepository repository, IPersonRepository clientRepository, IMapper mapper) : IVehicleService
{
    public async Task<Response<VehicleDto>> CreateAsync(CreateNewVehicleRequest request, CancellationToken cancellationToken)
    {
        var foundClient = await clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (foundClient is null)
        {
            return ResponseFactory.Fail<VehicleDto>(new FluentResults.Error("Client not found"), System.Net.HttpStatusCode.NotFound);
        }

        if (foundClient.PersonType != EPersonType.Client)
        {
            return ResponseFactory.Fail<VehicleDto>(new FluentResults.Error("Only clients are allowed to register a vehicle"), System.Net.HttpStatusCode.BadRequest);
        }

        if (!IsValidLicensePlate(request.LicensePlate))
        {
            return ResponseFactory.Fail<VehicleDto>(new FluentResults.Error("Invalid license plate format"), System.Net.HttpStatusCode.BadRequest);
        }

        var mapperEntity = mapper.Map<Vehicle>(request);
        var createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<VehicleDto>(createdEntity), System.Net.HttpStatusCode.Created);
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

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        if (foundEntity is null) return ResponseFactory.Fail(new FluentResults.Error("Vehicle not found"), System.Net.HttpStatusCode.NotFound);
        await repository.DeleteAsync(foundEntity, cancellationToken);
        return ResponseFactory.Ok(System.Net.HttpStatusCode.NoContent);
    }

    public async Task<Response<Paginate<VehicleDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var response = await repository.GetAllAsync(paginatedRequest, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<VehicleDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }

    public async Task<Response<VehicleDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        return foundEntity != null
            ? ResponseFactory.Ok(mapper.Map<VehicleDto>(foundEntity))
            : ResponseFactory.Fail<VehicleDto>(new FluentResults.Error("Vehicle Not Found"), System.Net.HttpStatusCode.NotFound);
    }

    public async Task<Response<VehicleDto>> UpdateAsync(UpdateOneVehicleInput input, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(input.Id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail<VehicleDto>(new FluentResults.Error("Vehicle not found"), System.Net.HttpStatusCode.NotFound);
        }

        _ = foundEntity.Update(input.ManufactureYear, input.LicensePlate, input.Brand, input.Model);
        var updatedEntity = await repository.UpdateAsync(foundEntity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<VehicleDto>(updatedEntity));
    }
}
