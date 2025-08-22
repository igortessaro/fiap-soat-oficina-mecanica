using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Stock;

public sealed class UpdateStockHandler(IMapper mapper, ISupplyRepository supplyRepository) : IRequestHandler<UpdateStockCommand, Response<SupplyDto>>
{
    public async Task<Response<SupplyDto>> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var entity = await supplyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return ResponseFactory.Fail<SupplyDto>("Supply not found", HttpStatusCode.NotFound);
        await supplyRepository.UpdateAsync(request.Adding ? entity.AddToStock(request.Quantity) : entity.RemoveFromStock(request.Quantity), cancellationToken);
        return ResponseFactory.Ok(mapper.Map<SupplyDto>(entity));
    }
}
