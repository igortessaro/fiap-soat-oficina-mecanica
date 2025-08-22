using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Get;

public sealed class GetServiceOrderByIdHandler(IMapper mapper, IServiceOrderRepository repository) : IRequestHandler<GetServiceOrderByIdQuery, Response<ServiceOrderDto>>
{
    public async Task<Response<ServiceOrderDto>> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetDetailedAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(mapper.Map<ServiceOrderDto>(entity))
            : ResponseFactory.Fail<ServiceOrderDto>("Service Order Not Found", HttpStatusCode.NotFound);
    }
}
