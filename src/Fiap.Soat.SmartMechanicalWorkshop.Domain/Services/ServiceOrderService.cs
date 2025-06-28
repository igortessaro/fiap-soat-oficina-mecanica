using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class ServiceOrderService(
    IMapper mapper,
    IServiceOrderRepository repository,
    IClientRepository clientRepository,
    IVehicleRepository vehicleRepository,
    IAvailableServiceRepository availableServiceRepository,
    IServiceOrderAvailableServiceRepository serviceOrderAvailableServiceRepository) : IServiceOrderService
{
    public async Task<Response<ServiceOrderDto>> CreateAsync(CreateServiceOrderRequest request, CancellationToken cancellationToken)
    {
        var mapperEntity = mapper.Map<ServiceOrder>(request);
        if (!await clientRepository.AnyAsync(x => x.Id == mapperEntity.ClientId, cancellationToken))
        {
            return Response<ServiceOrderDto>.Fail(new FluentResults.Error("Client not found"), System.Net.HttpStatusCode.NotFound);
        }

        if (!await vehicleRepository.AnyAsync(x => x.Id == mapperEntity.VehicleId, cancellationToken))
        {
            return Response<ServiceOrderDto>.Fail(new FluentResults.Error("Vehicle not found"), System.Net.HttpStatusCode.NotFound);
        }

        foreach (var serviceId in request.ServiceIds)
        {
            var availableService = await availableServiceRepository.GetByIdAsync(serviceId, cancellationToken);
            if (availableService is null)
            {
                return Response<ServiceOrderDto>.Fail(new FluentResults.Error($"Service with Id {serviceId} not found"), System.Net.HttpStatusCode.NotFound);
            }

            _ = mapperEntity.AddAvailableService(availableService);
        }

        var createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
        return Response<ServiceOrderDto>.Ok(mapper.Map<ServiceOrderDto>(createdEntity), System.Net.HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        if (foundEntity is null)
        {
            return Response.Fail(new FluentResults.Error("Service Order not found"), System.Net.HttpStatusCode.NotFound);
        }

        await repository.DeleteAsync(foundEntity, cancellationToken);
        return Response.Ok();
    }

    public async Task<Response<ServiceOrderDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        return foundEntity != null
            ? Response<ServiceOrderDto>.Ok(mapper.Map<ServiceOrderDto>(foundEntity))
            : Response<ServiceOrderDto>.Fail(new FluentResults.Error("Service Order Not Found"), System.Net.HttpStatusCode.NotFound);
    }

    public async Task<Response<ServiceOrderDto>> UpdateAsync(UpdateOneServiceOrderInput input, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetAsync(input.Id, cancellationToken);
        if (foundEntity is null)
        {
            return Response<ServiceOrderDto>.Fail(new FluentResults.Error("Service Order not found"), System.Net.HttpStatusCode.NotFound);
        }

        await serviceOrderAvailableServiceRepository.DeleteRangeAsync(foundEntity.ServiceOrderAvailableServices, cancellationToken);
        foreach (var service in input.ServiceIds)
        {
            var foundService = await availableServiceRepository.GetByIdAsync(service, cancellationToken);
            if (foundService is null)
            {
                return Response<ServiceOrderDto>.Fail(new FluentResults.Error($"Available Service with ID {service} not found"), System.Net.HttpStatusCode.NotFound);
            }
            _ = foundEntity.AddAvailableService(foundService);
        }
        var updatedEntity = await repository.UpdateAsync(foundEntity.Update(input.Title, input.Description), cancellationToken);
        return Response<ServiceOrderDto>.Ok(mapper.Map<ServiceOrderDto>(updatedEntity));
    }

    public async Task<Response<Paginate<ServiceOrderDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var response = await repository.GetAllAsync(paginatedRequest, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<ServiceOrderDto>>(response);
        return Response<Paginate<ServiceOrderDto>>.Ok(mappedResponse);
    }
}
