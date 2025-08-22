using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.List;

public sealed class ListServiceOrdersHandler(
    IMapper mapper,
    IServiceOrderRepository serviceOrderRepository) : IRequestHandler<ListServiceOrdersQuery, Response<Paginate<ServiceOrderDto>>>
{
    public async Task<Response<Paginate<ServiceOrderDto>>> Handle(ListServiceOrdersQuery request, CancellationToken cancellationToken)
    {
        string[] includes = [nameof(ServiceOrder.Client), nameof(ServiceOrder.Vehicle), nameof(ServiceOrder.AvailableServices)];
        var paginatedRequest = new PaginatedRequest(request.PageNumber, request.PageSize);
        var response = request.PersonId.HasValue
            ? await serviceOrderRepository.GetAllAsync(includes, x => x.ClientId == request.PersonId, paginatedRequest, cancellationToken)
            : await serviceOrderRepository.GetAllAsync(includes, paginatedRequest, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<ServiceOrderDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }
}
