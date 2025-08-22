using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Create;

public sealed class CreateSupplyHandler(IMapper mapper, ISupplyRepository supplyRepository) : IRequestHandler<CreateSupplyCommand, Response<SupplyDto>>
{
    public async Task<Response<SupplyDto>> Handle(CreateSupplyCommand request, CancellationToken cancellationToken)
    {
        if (await supplyRepository.AnyAsync(x => request.Name.ToLower().Equals(x.Name.ToLower()), cancellationToken))
        {
            return ResponseFactory.Fail<SupplyDto>($"Supply with name {request.Name} already exists", HttpStatusCode.Conflict);
        }

        var entity = mapper.Map<Supply>(request);
        var createdEntity = await supplyRepository.AddAsync(entity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<SupplyDto>(createdEntity), HttpStatusCode.Created);
    }
}
