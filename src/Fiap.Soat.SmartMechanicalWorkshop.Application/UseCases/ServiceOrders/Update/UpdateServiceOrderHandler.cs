using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Shared.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;

public sealed class UpdateServiceOrderHandler(
    IServiceOrderRepository serviceOrderRepository,
    IAvailableServiceRepository availableServiceRepository,
    ITelemetryService telemetryService) : IRequestHandler<UpdateServiceOrderCommand, Response<ServiceOrder>>
{
    public async Task<Response<ServiceOrder>> Handle(UpdateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await serviceOrderRepository.GetAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail<ServiceOrder>("Service Order not found", HttpStatusCode.NotFound);
        }

        var services = new List<AvailableService>();
        foreach (var service in request.ServiceIds)
        {
            var foundService = await availableServiceRepository.GetByIdAsync(service, cancellationToken);
            if (foundService is null)
            {
                return ResponseFactory.Fail<ServiceOrder>($"Available Service with ID {service} not found",
                    HttpStatusCode.NotFound);
            }

            services.Add(foundService);
        }

        var updatedEntity = await serviceOrderRepository.UpdateAsync(request.Id, request.Title, request.Description, services, cancellationToken);

        telemetryService.RecordServiceOrderEvent(
            updatedEntity.Id,
            updatedEntity.ClientId,
            updatedEntity.Status.ToString(),
            "updated",
            new Dictionary<string, object>
            {
                { "servicesCount", services.Count }
            });

        return ResponseFactory.Ok(updatedEntity);
    }
}
