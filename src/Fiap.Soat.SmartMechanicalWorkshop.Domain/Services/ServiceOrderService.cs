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
     IConfiguration configuration
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

        string? emailBaseUrl = configuration["Email:BaseUrl"];
        string approveUrl = $"{emailBaseUrl}/api/v1/serviceorders/{foundEntity.Id}/approve";
        string rejectUrl = $"{emailBaseUrl}/api/v1/serviceorders/{foundEntity.Id}/reject";

        string html = $@"
  <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #ccc; padding: 20px;'>
    <h2 style='color: #007BFF;'>Ordem de Serviço - Oficina Smart</h2>
    <p>Olá, {foundEntity.Client.Fullname}!</p>

    <p>Segue abaixo os detalhes da sua ordem de serviço:</p>

    <ul style='list-style: none; padding-left: 0;'>
      <li><strong>ID:</strong> {foundEntity.Id}</li>
      <li><strong>Título:</strong> {foundEntity.Title}</li>
      <li><strong>Descrição:</strong> {foundEntity.Description}</li>
      <li><strong>Status Atual:</strong> {foundEntity.Status}</li>
      <li><strong>Data Entrada:</strong> {foundEntity.VehicleCheckInDate:dd/MM/yyyy}</li>
      <li><strong>Data Saída:</strong> {(foundEntity.VehicleCheckOutDate.HasValue ? foundEntity.VehicleCheckOutDate.Value.ToString("dd/MM/yyyy") : "—")}</li>
    </ul>

    <h4>Informações do veículo</h4>
    <ul style='list-style: none; padding-left: 0;'>
      <li><strong>Marca:</strong> {foundEntity.Vehicle.Brand}</li>
      <li><strong>Modelo:</strong> {foundEntity.Vehicle.Model}</li>
      <li><strong>Ano:</strong> {foundEntity.Vehicle.ManufactureYear}</li>
      <li><strong>Placa:</strong> {foundEntity.Vehicle.LicensePlate}</li>
    </ul>

    <h4>Serviços solicitados:</h4>
    <ul>
      {string.Join("", foundEntity.ServiceOrderAvailableServices.Select(s => $"<li>{s.AvailableServiceId}</li>"))}
    </ul>

    <div style='margin-top: 30px;'>
      <a href='{approveUrl}' style='padding: 12px 20px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px; margin-right: 10px;'>Aprovar</a>
      <a href='{rejectUrl}' style='padding: 12px 20px; background-color: #dc3545; color: white; text-decoration: none; border-radius: 5px;'>Reprovar</a>
    </div>

    <p style='margin-top: 40px;'>Obrigado por escolher a <strong>Oficina Smart</strong>! Em caso de dúvidas, entre em contato pelo WhatsApp: <a href='tel:+5500000000000'>+55 00 0000-0000</a>.</p>
  </div>";


        bool response = await emailService.SendEmailAsync(foundEntity.Client.Email.Address, "Envio de orçamento de serviço(s)", html);
        return response ? ResponseFactory.Ok(HttpStatusCode.Accepted)
            : ResponseFactory.Fail(new FluentResults.Error("Not possible to send email"), System.Net.HttpStatusCode.InternalServerError);


    }
}
