using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;
using System.Linq.Expressions;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.List;

public sealed class ListServiceOrdersHandler(
    IMapper mapper,
    IServiceOrderRepository serviceOrderRepository) : IRequestHandler<ListServiceOrdersQuery, Response<Paginate<ServiceOrderDto>>>
{
    public async Task<Response<Paginate<ServiceOrderDto>>> Handle(ListServiceOrdersQuery request, CancellationToken cancellationToken)
    {
        string[] includes = [nameof(ServiceOrder.Client), nameof(ServiceOrder.Vehicle), nameof(ServiceOrder.AvailableServices)];
        var paginatedRequest = new PaginatedRequest(request.PageNumber, request.PageSize);
        Func<IQueryable<ServiceOrder>, IOrderedQueryable<ServiceOrder>> orderBy = q =>
            q.OrderBy(x =>

                x.Status == ServiceOrderStatus.InProgress ? 1 :
                x.Status == ServiceOrderStatus.WaitingApproval ? 2 :
                x.Status == ServiceOrderStatus.UnderDiagnosis ? 3 :
                x.Status == ServiceOrderStatus.Received ? 4 : 5
            )
            .ThenBy(x => x.CreatedAt);

        var excludedStatuses = new[] { ServiceOrderStatus.Delivered, ServiceOrderStatus.Completed, ServiceOrderStatus.Cancelled, ServiceOrderStatus.Rejected };

        Expression<Func<ServiceOrder, bool>> predicate = request.PersonId.HasValue
        ? x => x.ClientId == request.PersonId && !excludedStatuses.Contains(x.Status)
        : x => !excludedStatuses.Contains(x.Status);

        var response = await serviceOrderRepository.GetAllAsync(includes, predicate, paginatedRequest, cancellationToken, orderBy);
        var mappedResponse = mapper.Map<Paginate<ServiceOrderDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }
}
