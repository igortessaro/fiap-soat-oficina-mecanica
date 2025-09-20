using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.Delete;

public sealed class DeleteAvailableServiceHandler(IAvailableServiceRepository availableServiceRepository) : IRequestHandler<DeleteAvailableServiceCommand, Response>
{
    public async Task<Response> Handle(DeleteAvailableServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await availableServiceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail("AvailableService not found", HttpStatusCode.NotFound);
        }

        await availableServiceRepository.DeleteAsync(entity, cancellationToken);
        return ResponseFactory.Ok(HttpStatusCode.NoContent);
    }
}
