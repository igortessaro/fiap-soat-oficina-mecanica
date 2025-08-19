using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Get;

public sealed class GetSupplyByIdHandler(IMapper mapper, ISupplyRepository supplyRepository) : IRequestHandler<GetSupplyByIdQuery, Response<SupplyDto>>
{
    public async Task<Response<SupplyDto>> Handle(GetSupplyByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await supplyRepository.GetByIdAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(mapper.Map<SupplyDto>(entity))
            : ResponseFactory.Fail<SupplyDto>("Supply Not Found", HttpStatusCode.NotFound);
    }
}
