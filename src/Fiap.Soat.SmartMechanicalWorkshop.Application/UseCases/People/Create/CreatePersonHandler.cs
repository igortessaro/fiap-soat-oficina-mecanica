using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Create;

public sealed class CreatePersonHandler(IMapper mapper, IPersonRepository personRepository) : IRequestHandler<CreatePersonCommand, Response<PersonDto>>
{
    public async Task<Response<PersonDto>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var mapperEntity = mapper.Map<Person>(request);
        mapperEntity.Validate();
        var createdEntity = await personRepository.AddAsync(mapperEntity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<PersonDto>(createdEntity), HttpStatusCode.Created);
    }
}
