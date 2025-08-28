using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.ServiceOrders.Get;

public sealed class GetServiceOrderByIdHandler(IServiceOrderRepository repository) : IRequestHandler<GetServiceOrderByIdQuery, Response<ServiceOrder>>
{
    public async Task<Response<ServiceOrder>> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetDetailedAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(entity)
            : ResponseFactory.Fail<ServiceOrder>("Service Order Not Found", HttpStatusCode.NotFound);
    }
}
