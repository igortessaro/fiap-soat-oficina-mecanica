using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Update;

public sealed class UpdateAvailableServiceHandler(
    IAvailableServiceRepository availableServiceRepository,
    ISupplyRepository supplyRepository) : IRequestHandler<UpdateAvailableServiceCommand, Response<AvailableService>>
{
    public async Task<Response<AvailableService>> Handle(UpdateAvailableServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await availableServiceRepository.GetAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail<AvailableService>("AvailableService not found", HttpStatusCode.NotFound);
        }

        if (!request.Name.Equals(entity.Name, StringComparison.CurrentCultureIgnoreCase) &&
            await availableServiceRepository.AnyAsync(x => request.Name.ToLower().Equals(x.Name.ToLower()), cancellationToken))
        {
            return ResponseFactory.Fail<AvailableService>($"AvailableService with name {request.Name} already exists", HttpStatusCode.Conflict);
        }

        foreach (var supply in request.Supplies)
        {
            var foundSupply = await supplyRepository.GetByIdAsync(supply.SupplyId, cancellationToken);
            if (foundSupply is null)
            {
                return ResponseFactory.Fail<AvailableService>($"Supply with ID {supply} not found", HttpStatusCode.NotFound);
            }
        }

        var supplies = request.Supplies.Select(x => new ServiceSupplyDto(request.Id, x.SupplyId, x.Quantity)).ToList();
        var updated = await availableServiceRepository.UpdateAsync(request.Id, request.Name, request.Price, supplies, cancellationToken);
        return ResponseFactory.Ok(updated);
    }
}
