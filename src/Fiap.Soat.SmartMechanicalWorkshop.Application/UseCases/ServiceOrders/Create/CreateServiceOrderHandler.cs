using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Create;

public sealed class CreateServiceOrderHandler(
    IMapper mapper,
    IServiceOrderRepository serviceOrderRepository,
    IPersonRepository personRepository,
    IAvailableServiceRepository availableServiceRepository,
    IVehicleRepository vehicleRepository) : IRequestHandler<CreateServiceOrderCommand, Response<ServiceOrder>>
{
    public async Task<Response<ServiceOrder>> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ServiceOrder>(request);
        if (!await personRepository.AnyAsync(x => x.Id == entity.ClientId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrder>("Person not found", HttpStatusCode.NotFound);
        }

        if (!await vehicleRepository.AnyAsync(x => x.Id == entity.VehicleId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrder>("Vehicle not found", HttpStatusCode.NotFound);
        }

        foreach (var serviceId in request.ServiceIds)
        {
            var availableService = await availableServiceRepository.GetByIdAsync(serviceId, cancellationToken);
            if (availableService is null)
            {
                return ResponseFactory.Fail<ServiceOrder>($"Service with Id {serviceId} not found",
                    HttpStatusCode.NotFound);
            }

            _ = entity.AddAvailableService(availableService);
        }

        var createdEntity = await serviceOrderRepository.AddAsync(entity, cancellationToken);
        return ResponseFactory.Ok(createdEntity, HttpStatusCode.Created);
    }
}
