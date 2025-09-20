using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Create;

public sealed class CreateSupplyHandler(IMapper mapper, ISupplyRepository supplyRepository) : IRequestHandler<CreateSupplyCommand, Response<Supply>>
{
    public async Task<Response<Supply>> Handle(CreateSupplyCommand request, CancellationToken cancellationToken)
    {
        if (await supplyRepository.AnyAsync(x => request.Name.ToLower().Equals(x.Name.ToLower()), cancellationToken))
        {
            return ResponseFactory.Fail<Supply>($"Supply with name {request.Name} already exists", HttpStatusCode.Conflict);
        }

        var entity = mapper.Map<Supply>(request);
        var createdEntity = await supplyRepository.AddAsync(entity, cancellationToken);
        return ResponseFactory.Ok(createdEntity, HttpStatusCode.Created);
    }
}
