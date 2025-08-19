using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Update;

public sealed class UpdateServiceOrderHandler(
    IMapper mapper,
    IServiceOrderRepository serviceOrderRepository,
    IAvailableServiceRepository availableServiceRepository) : IRequestHandler<UpdateServiceOrderCommand, Response<ServiceOrderDto>>
{
    public async Task<Response<ServiceOrderDto>> Handle(UpdateServiceOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await serviceOrderRepository.GetAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail<ServiceOrderDto>("Service Order not found", HttpStatusCode.NotFound);
        }

        var services = new List<AvailableService>();
        foreach (var service in request.ServiceIds)
        {
            var foundService = await availableServiceRepository.GetByIdAsync(service, cancellationToken);
            if (foundService is null)
            {
                return ResponseFactory.Fail<ServiceOrderDto>($"Available Service with ID {service} not found",
                    HttpStatusCode.NotFound);
            }

            services.Add(foundService);
        }

        var updatedEntity = await serviceOrderRepository.UpdateAsync(request.Id, request.Title, request.Description, services, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(updatedEntity));
    }
}
