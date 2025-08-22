using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;

public sealed class ListPeopleHandler(IMapper mapper, IPersonRepository personRepository) : IRequestHandler<ListPeopleQuery, Response<Paginate<PersonDto>>>
{
    public async Task<Response<Paginate<PersonDto>>> Handle(ListPeopleQuery request, CancellationToken cancellationToken)
    {
        var response = await personRepository.GetAllAsync([nameof(Person.Vehicles), nameof(Person.Address)], request, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<PersonDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }
}
