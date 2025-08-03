using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.ExternalServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class ServiceOrderService(
    IMapper mapper,
    IServiceOrderRepository repository,
    IPersonRepository personRepository,
    IVehicleRepository vehicleRepository,
    IAvailableServiceRepository availableServiceRepository) : IServiceOrderService
{
    public async Task<Response<ServiceOrderDto>> CreateAsync(CreateServiceOrderRequest request, CancellationToken cancellationToken)
    {
        var mapperEntity = mapper.Map<ServiceOrder>(request);
        if (!await personRepository.AnyAsync(x => x.Id == mapperEntity.ClientId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Person not found"), System.Net.HttpStatusCode.NotFound);
        }

        if (!await vehicleRepository.AnyAsync(x => x.Id == mapperEntity.VehicleId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Vehicle not found"), System.Net.HttpStatusCode.NotFound);
        }

        foreach (var serviceId in request.ServiceIds)
        {
            var availableService = await availableServiceRepository.GetByIdAsync(serviceId, cancellationToken);
            if (availableService is null)
            {
                return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error($"Service with Id {serviceId} not found"),
                    System.Net.HttpStatusCode.NotFound);
            }

            _ = mapperEntity.AddAvailableService(availableService);
        }

        var createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(createdEntity), System.Net.HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail(new FluentResults.Error("Service Order not found"), System.Net.HttpStatusCode.NotFound);
        }

        await repository.DeleteAsync(foundEntity, cancellationToken);
        return ResponseFactory.Ok(System.Net.HttpStatusCode.NoContent);
    }

    public async Task<Response<ServiceOrderDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetDetailedAsync(id, cancellationToken);
        return foundEntity != null
            ? ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(foundEntity))
            : ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Service Order Not Found"), System.Net.HttpStatusCode.NotFound);
    }

    public async Task<Response<ServiceOrderDto>> UpdateAsync(UpdateOneServiceOrderInput input, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetAsync(input.Id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Service Order not found"), System.Net.HttpStatusCode.NotFound);
        }

        var services = new List<AvailableService>();
        foreach (var service in input.ServiceIds)
        {
            var foundService = await availableServiceRepository.GetByIdAsync(service, cancellationToken);
            if (foundService is null)
            {
                return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error($"Available Service with ID {service} not found"),
                    System.Net.HttpStatusCode.NotFound);
            }

            services.Add(foundService);
        }

        var updatedEntity = await repository.UpdateAsync(input.Id, input.Title, input.Description, services, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(updatedEntity));
    }

    public async Task<Response<Paginate<ServiceOrderDto>>> GetAllAsync(Guid? personId, PaginatedRequest paginatedRequest,
        CancellationToken cancellationToken)
    {
        string[] includes = [nameof(ServiceOrder.Client), nameof(ServiceOrder.Vehicle), nameof(ServiceOrder.AvailableServices)];
        var response = personId.HasValue
            ? await repository.GetAllAsync(includes, x => x.ClientId == personId, paginatedRequest, cancellationToken)
            : await repository.GetAllAsync(includes, paginatedRequest, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<ServiceOrderDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }

    public async Task<Response<ServiceOrderDto>> PatchAsync(PatchOneServiceOrderInput input, CancellationToken cancellationToken)
    {
        var foundServiceOrder = await repository.GetByIdAsync(input.Id, cancellationToken);
        if (foundServiceOrder is null)
        {
            return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Service Order not found"), System.Net.HttpStatusCode.NotFound);
        }

        _ = foundServiceOrder.ChangeStatus(input.Status);
        _ = await repository.UpdateAsync(foundServiceOrder, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(await repository.GetDetailedAsync(input.Id, cancellationToken)));
    }
}
