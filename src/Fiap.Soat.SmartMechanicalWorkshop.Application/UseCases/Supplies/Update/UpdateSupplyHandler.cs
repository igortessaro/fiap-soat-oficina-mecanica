using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Update;

public sealed class UpdateSupplyHandler(ISupplyRepository supplyRepository) : IRequestHandler<UpdateSupplyCommand, Response<Supply>>
{
    public async Task<Response<Supply>> Handle(UpdateSupplyCommand request, CancellationToken cancellationToken)
    {
        var entity = await supplyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail<Supply>("Supply not found", HttpStatusCode.NotFound);
        }

        var updatedEntity = await supplyRepository.UpdateAsync(entity.Update(request.Name, request.Price, request.Quantity), cancellationToken);
        return ResponseFactory.Ok(updatedEntity);
    }
}
