using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.People.Update;

public sealed class UpdatePersonHandler(IMapper mapper, IPersonRepository personRepository) : IRequestHandler<UpdatePersonCommand, Response<Person>>
{
    public async Task<Response<Person>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var entity = await personRepository.GetAsync(request.Id, cancellationToken);
        if (entity is null)
        {
            return ResponseFactory.Fail<Person>("Person not found", HttpStatusCode.NotFound);
        }

        var phone = mapper.Map<Phone>(request.Phone);
        var address = mapper.Map<Address>(request.Address);
        var updatePerson = entity.Update(request.Fullname, request.Document, request.PersonType, request.EmployeeRole, request.Email, request.Password, phone, address);
        entity.Validate();
        var updatedEntity = await personRepository.UpdateAsync(updatePerson, cancellationToken);
        return ResponseFactory.Ok(updatedEntity);
    }
}
