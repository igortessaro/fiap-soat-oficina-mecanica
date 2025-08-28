using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Delete;

public sealed class DeleteVehicleHandler(IVehicleRepository repository) : IRequestHandler<DeleteVehicleCommand, Response>
{
    public async Task<Response> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail("Vehicle not found", HttpStatusCode.NotFound);
        }

        await repository.DeleteAsync(entity, cancellationToken);
        return ResponseFactory.Ok(HttpStatusCode.NoContent);
    }
}
