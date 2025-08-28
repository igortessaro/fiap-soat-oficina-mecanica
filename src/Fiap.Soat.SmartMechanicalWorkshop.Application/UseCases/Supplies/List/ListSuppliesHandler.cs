using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.List;

public sealed class ListSuppliesHandler(ISupplyRepository supplyRepository) : IRequestHandler<ListSuppliesQuery, Response<Paginate<Supply>>>
{
    public async Task<Response<Paginate<Supply>>> Handle(ListSuppliesQuery request, CancellationToken cancellationToken)
    {
        var response = await supplyRepository.GetAllAsync(request, cancellationToken);
        return ResponseFactory.Ok(response);
    }
}
