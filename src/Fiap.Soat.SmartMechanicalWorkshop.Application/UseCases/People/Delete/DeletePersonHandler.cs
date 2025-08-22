using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Delete;

public sealed class DeletePersonHandler(
    IPersonRepository personRepository,
    IAddressRepository addressRepository) : IRequestHandler<DeletePersonCommand, Response>
{
    public async Task<Response> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = await personRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail("Person not found", HttpStatusCode.NotFound);
        }

        var address = await addressRepository.GetByIdAsync(entity.AddressId, cancellationToken);
        await personRepository.DeleteAsync(entity, cancellationToken);
        await addressRepository.DeleteAsync(address!, cancellationToken);
        return ResponseFactory.Ok(HttpStatusCode.NoContent);
    }
}
