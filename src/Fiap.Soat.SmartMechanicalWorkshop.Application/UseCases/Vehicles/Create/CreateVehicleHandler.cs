using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Create;

public sealed class CreateVehicleHandler(IMapper mapper, IVehicleRepository repository, IPersonRepository personRepository) : IRequestHandler<CreateVehicleCommand, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await personRepository.GetByIdAsync(request.PersonId, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail<VehicleDto>("Person not found", HttpStatusCode.NotFound);
        }

        if (entity.PersonType != PersonType.Client)
        {
            return ResponseFactory.Fail<VehicleDto>("Only clients are allowed to register a vehicle");
        }

        var mapperEntity = mapper.Map<Vehicle>(request);
        if (!mapperEntity.LicensePlate.IsValid())
        {
            return ResponseFactory.Fail<VehicleDto>("Invalid license plate format");
        }

        var createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<VehicleDto>(createdEntity), HttpStatusCode.Created);
    }
}
