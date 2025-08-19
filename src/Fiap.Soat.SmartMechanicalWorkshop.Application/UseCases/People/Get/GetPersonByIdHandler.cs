using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Get;

public sealed class GetPersonByIdHandler(IMapper mapper, IPersonRepository personRepository) : IRequestHandler<GetPersonByIdQuery, Response<PersonDto>>
{
    public async Task<Response<PersonDto>> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await personRepository.GetDetailedByIdAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(mapper.Map<PersonDto>(entity))
            : ResponseFactory.Fail<PersonDto>("Person Not Found", HttpStatusCode.NotFound);
    }
}
