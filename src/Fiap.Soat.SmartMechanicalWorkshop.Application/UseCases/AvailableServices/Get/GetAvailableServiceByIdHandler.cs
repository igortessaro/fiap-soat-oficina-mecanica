using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Get;

public sealed class GetAvailableServiceByIdHandler(IAvailableServiceRepository availableServiceRepository)
    : IRequestHandler<GetAvailableServiceByIdQuery, Response<AvailableService>>
{
    public async Task<Response<AvailableService>> Handle(GetAvailableServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await availableServiceRepository.GetAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(entity)
            : ResponseFactory.Fail<AvailableService>("AvailableService Not Found", HttpStatusCode.NotFound);
    }
}
