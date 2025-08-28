using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Get;

public sealed class GetSupplyByIdHandler(ISupplyRepository supplyRepository) : IRequestHandler<GetSupplyByIdQuery, Response<Supply>>
{
    public async Task<Response<Supply>> Handle(GetSupplyByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await supplyRepository.GetByIdAsync(request.Id, cancellationToken);
        return entity != null ? ResponseFactory.Ok(entity) : ResponseFactory.Fail<Supply>("Supply Not Found", HttpStatusCode.NotFound);
    }
}
