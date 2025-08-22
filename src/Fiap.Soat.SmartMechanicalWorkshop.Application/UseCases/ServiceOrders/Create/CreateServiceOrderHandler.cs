using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
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
    IVehicleRepository vehicleRepository) : IRequestHandler<CreateServiceOrderCommand, Response<ServiceOrderDto>>
{
    public async Task<Response<ServiceOrderDto>> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<ServiceOrder>(request);
        if (!await personRepository.AnyAsync(x => x.Id == entity.ClientId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrderDto>("Person not found", HttpStatusCode.NotFound);
        }

        if (!await vehicleRepository.AnyAsync(x => x.Id == entity.VehicleId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrderDto>("Vehicle not found", HttpStatusCode.NotFound);
        }

        foreach (var serviceId in request.ServiceIds)
        {
            var availableService = await availableServiceRepository.GetByIdAsync(serviceId, cancellationToken);
            if (availableService is null)
            {
                return ResponseFactory.Fail<ServiceOrderDto>($"Service with Id {serviceId} not found",
                    HttpStatusCode.NotFound);
            }

            _ = entity.AddAvailableService(availableService);
        }

        var createdEntity = await serviceOrderRepository.AddAsync(entity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(createdEntity), HttpStatusCode.Created);
    }
}
