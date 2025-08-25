using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Create;

public sealed class CreateAvailableServiceHandler(
    IMapper mapper,
    IAvailableServiceRepository availableServiceRepository,
    ISupplyRepository supplyRepository) : IRequestHandler<CreateAvailableServiceCommand, Response<AvailableService>>
{
    public async Task<Response<AvailableService>> Handle(CreateAvailableServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<AvailableService>(request);
        if (await availableServiceRepository.AnyAsync(x => request.Name.ToLower().Equals(x.Name.ToLower()), cancellationToken))
        {
            return ResponseFactory.Fail<AvailableService>($"AvailableService with name {request.Name} already exists", HttpStatusCode.Conflict);
        }

        if (!request.Supplies.Any())
        {
            return ResponseFactory.Ok(await availableServiceRepository.AddAsync(entity, cancellationToken), HttpStatusCode.Created);
        }

        foreach (var supply in request.Supplies)
        {
            var foundSupply = await supplyRepository.GetByIdAsync(supply.SupplyId, cancellationToken);
            if (foundSupply is null)
            {
                return ResponseFactory.Fail<AvailableService>($"Supply with ID {supply} not found", HttpStatusCode.NotFound);
            }

            _ = entity.AddSupply(supply.SupplyId, supply.Quantity);
        }

        var created = await availableServiceRepository.AddAsync(entity, cancellationToken);
        return ResponseFactory.Ok(created, HttpStatusCode.Created);
    }
}
