using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Delete;

public sealed class DeleteSupplyHandler(ISupplyRepository supplyRepository) : IRequestHandler<DeleteSupplyCommand, Response>
{
    public async Task<Response> Handle(DeleteSupplyCommand request, CancellationToken cancellationToken)
    {
        var foundEntity = await supplyRepository.GetByIdAsync(request.Id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail("Supply not found", HttpStatusCode.NotFound);
        }

        await supplyRepository.DeleteAsync(foundEntity, cancellationToken);
        return ResponseFactory.Ok(HttpStatusCode.NoContent);
    }
}
