using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.ExternalServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class ServiceOrderService(
    IMapper mapper,
    IServiceOrderRepository repository,
    IClientRepository clientRepository,
    IVehicleRepository vehicleRepository,
    IAvailableServiceRepository availableServiceRepository,
    IServiceOrderAvailableServiceRepository serviceOrderAvailableServiceRepository,
     IEmailService emailService,
     IEmailTemplateProvider emailTemplateProvider
     ) : IServiceOrderService
{
    public async Task<Response<ServiceOrderDto>> CreateAsync(CreateServiceOrderRequest request, CancellationToken cancellationToken)
    {
        var mapperEntity = mapper.Map<ServiceOrder>(request);
        if (!await clientRepository.AnyAsync(x => x.Id == mapperEntity.ClientId, cancellationToken))
        {
            return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Client not found"), System.Net.HttpStatusCode.NotFound);
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
                return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error($"Service with Id {serviceId} not found"), System.Net.HttpStatusCode.NotFound);
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

        await serviceOrderAvailableServiceRepository.DeleteRangeAsync(foundEntity.ServiceOrderAvailableServices, cancellationToken);

        if (input.ServiceIds != null)
        {
            foreach (var service in input.ServiceIds)
            {
                var foundService = await availableServiceRepository.GetByIdAsync(service, cancellationToken);
                if (foundService is null)
                {
                    return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error($"Available Service with ID {service} not found"), System.Net.HttpStatusCode.NotFound);
                }
                _ = foundEntity.AddAvailableService(foundService);
            }
        }

        var updatedObj = foundEntity.Update(input.Title, input.Description, input.ServiceOrderStatus, input.VehicleCheckInDate, input.VehicleCheckOutDate);
        var updatedEntity = await repository.UpdateAsync(updatedObj, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(updatedEntity));
    }

    public async Task<Response<Paginate<ServiceOrderDto>>> GetAllAsync(Guid? clientId, PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var response = clientId.HasValue ?
            await repository.GetAllAsync(x => x.ClientId == clientId, paginatedRequest, cancellationToken) :
            await repository.GetAllAsync(paginatedRequest, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<ServiceOrderDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }

    public async Task<Response> SendForApprovalAsync(SendServiceOrderApprovalRequest request, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetDetailedAsync(request.Id, cancellationToken);

        if (foundEntity is null)
        {
            return ResponseFactory.Fail(new FluentResults.Error("Service Order Not Found"), System.Net.HttpStatusCode.NotFound);
        }

        string html = emailTemplateProvider.GetTemplate(foundEntity);

        bool response = await emailService.SendEmailAsync(foundEntity.Client.Email.Address, "Envio de orçamento de serviço(s)", html);
        return response ? ResponseFactory.Ok(HttpStatusCode.Accepted)
            : ResponseFactory.Fail(new FluentResults.Error("Not possible to send email"), System.Net.HttpStatusCode.InternalServerError);

    }
}
