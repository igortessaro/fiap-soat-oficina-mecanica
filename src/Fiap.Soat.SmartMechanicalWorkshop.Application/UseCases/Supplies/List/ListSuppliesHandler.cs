using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.List;

public sealed class ListSuppliesHandler(IMapper mapper, ISupplyRepository supplyRepository) : IRequestHandler<ListSuppliesQuery, Response<Paginate<SupplyDto>>>
{
    public async Task<Response<Paginate<SupplyDto>>> Handle(ListSuppliesQuery request, CancellationToken cancellationToken)
    {
        var response = await supplyRepository.GetAllAsync(request, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<SupplyDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }
}
