using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.List;

public sealed class ListPeopleHandler(IPersonRepository personRepository) : IRequestHandler<ListPeopleQuery, Response<Paginate<Person>>>
{
    public async Task<Response<Paginate<Person>>> Handle(ListPeopleQuery request, CancellationToken cancellationToken)
    {
        var response = await personRepository.GetAllAsync([nameof(Person.Vehicles), nameof(Person.Address)], request, cancellationToken);
        return ResponseFactory.Ok(response);
    }
}
