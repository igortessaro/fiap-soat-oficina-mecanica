using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.ExternalServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class ServiceOrderService(
    ILogger<ServiceOrderService> logger,
    IMapper mapper,
    IServiceOrderRepository repository,
    IPersonRepository personRepository,
    IVehicleRepository vehicleRepository,
    IAvailableServiceRepository availableServiceRepository,
    IEmailService emailService,
    IEmailTemplateProvider emailTemplateProvider) : IServiceOrderService
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

        // TODO: Adicionar validação para verificar se o cliente já possui uma ordem de serviço em andamento para o mesmo veículo.

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

        foundEntity.AvailableServices.Clear();

        if (input.ServiceIds != null)
        {
            foreach (var service in input.ServiceIds)
            {
                var foundService = await availableServiceRepository.GetByIdAsync(service, cancellationToken);
                if (foundService is null)
                {
                    return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error($"Available Service with ID {service} not found"),
                        System.Net.HttpStatusCode.NotFound);
                }

                _ = foundEntity.AddAvailableService(foundService);
            }
        }

        var updatedObj = foundEntity.Update(input.Title, input.Description, input.ServiceOrderStatus);
        var updatedEntity = await repository.UpdateAsync(updatedObj, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(updatedEntity));
    }

    public async Task<Response<Paginate<ServiceOrderDto>>> GetAllAsync(Guid? personId, PaginatedRequest paginatedRequest,
        CancellationToken cancellationToken)
    {
        var response = personId.HasValue
            ? await repository.GetAllAsync(x => x.ClientId == personId, paginatedRequest, cancellationToken)
            : await repository.GetAllAsync(paginatedRequest, cancellationToken);
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
        return response
            ? ResponseFactory.Ok(HttpStatusCode.Accepted)
            : ResponseFactory.Fail(new FluentResults.Error("Not possible to send email"), System.Net.HttpStatusCode.InternalServerError);
    }

    public async Task<Response<ServiceOrderDto>> ApproveOrderAsync(UpdateOneServiceOrderInput input, CancellationToken cancellationToken)
    {
        var foundServiceOrder = await repository.GetAsync(input.Id, cancellationToken);
        if (foundServiceOrder is null)
        {
            return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Service Order not found"), System.Net.HttpStatusCode.NotFound);
        }

        if (foundServiceOrder.Status != EServiceOrderStatus.WaitingApproval)
        {
            logger.LogWarning($"Service Order with ID {input.Id} is not in WaitingApproval status, current status: {foundServiceOrder.Status}");
            return ResponseFactory.Fail<ServiceOrderDto>(
                new FluentResults.Error($"Service Order with ID {input.Id} is not in WaitingApproval status, current status: {foundServiceOrder.Status}"),
                System.Net.HttpStatusCode.NotAcceptable
            );
        }

        return await UpdateAsync(
            new UpdateOneServiceOrderInput(input.Id, input.ServiceIds, input.Title, input.Description, EServiceOrderStatus.InProgress), cancellationToken);
    }

    public async Task<Response<ServiceOrderDto>> RejectOrderAsync(UpdateOneServiceOrderInput input, CancellationToken cancellationToken)
    {
        var foundServiceOrder = await repository.GetAsync(input.Id, cancellationToken);
        if (foundServiceOrder is null)
        {
            return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Service Order not found"), System.Net.HttpStatusCode.NotFound);
        }

        if (foundServiceOrder.Status != EServiceOrderStatus.WaitingApproval)
        {
            string logMessage = $"You can only change status if Service Order were in WaitingApproval status, current status: {foundServiceOrder.Status}";
            logger.LogWarning(logMessage);
            return ResponseFactory.Fail<ServiceOrderDto>(
                new FluentResults.Error(logMessage),
                System.Net.HttpStatusCode.NotAcceptable
            );
        }

        return await UpdateAsync(new UpdateOneServiceOrderInput(input.Id, input.ServiceIds, input.Title, input.Description, EServiceOrderStatus.Rejected),
            cancellationToken);
    }

    public async Task<Response<ServiceOrderDto>> PatchAsync(UpdateOneServiceOrderInput input, CancellationToken cancellationToken)
    {
        var foundServiceOrder = await repository.GetByIdAsync(input.Id, cancellationToken);
        if (foundServiceOrder is null)
        {
            return ResponseFactory.Fail<ServiceOrderDto>(new FluentResults.Error("Service Order not found"), System.Net.HttpStatusCode.NotFound);
        }

        _ = foundServiceOrder.ChangeStatus(input.ServiceOrderStatus!.Value);
        _ = await repository.UpdateAsync(foundServiceOrder, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(foundServiceOrder));
    }
}
