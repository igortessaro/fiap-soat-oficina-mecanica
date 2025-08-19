using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Update;

public sealed class UpdateSupplyHandler(IMapper mapper, ISupplyRepository supplyRepository) : IRequestHandler<UpdateSupplyCommand, Response<SupplyDto>>
{
    public async Task<Response<SupplyDto>> Handle(UpdateSupplyCommand request, CancellationToken cancellationToken)
    {
        var entity = await supplyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail<SupplyDto>("Supply not found", HttpStatusCode.NotFound);
        }

        var updatedEntity = await supplyRepository.UpdateAsync(entity.Update(request.Name, request.Price, request.Quantity), cancellationToken);
        return ResponseFactory.Ok(mapper.Map<SupplyDto>(updatedEntity));
    }
}
