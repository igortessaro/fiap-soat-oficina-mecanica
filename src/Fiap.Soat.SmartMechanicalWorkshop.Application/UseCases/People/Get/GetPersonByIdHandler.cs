using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Get;

public sealed class GetPersonByIdHandler(IPersonRepository personRepository) : IRequestHandler<GetPersonByIdQuery, Response<Person>>
{
    public async Task<Response<Person>> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await personRepository.GetDetailedByIdAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(entity)
            : ResponseFactory.Fail<Person>("Person Not Found", HttpStatusCode.NotFound);
    }
}
