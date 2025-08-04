using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Auth;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class PersonService(IMapper mapper, IPersonRepository repository, IAddressRepository addressRepository) : IPersonService
{
    public async Task<Response<PersonDto>> CreateAsync(CreatePersonRequest request, CancellationToken cancellationToken)
    {
        var mapperEntity = mapper.Map<Person>(request);
        mapperEntity.Validate();
        var createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<PersonDto>(createdEntity), HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail("Person not found", HttpStatusCode.NotFound);
        }

        var address = await addressRepository.GetByIdAsync(foundEntity.AddressId, cancellationToken);
        await repository.DeleteAsync(foundEntity, cancellationToken);
        await addressRepository.DeleteAsync(address!, cancellationToken);
        return ResponseFactory.Ok(HttpStatusCode.NoContent);
    }

    public async Task<Response<PersonDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetDetailedByIdAsync(id, cancellationToken);
        return foundEntity != null
            ? ResponseFactory.Ok(mapper.Map<PersonDto>(foundEntity))
            : ResponseFactory.Fail<PersonDto>("Person Not Found", HttpStatusCode.NotFound);
    }

    public async Task<Response<PersonDto>> UpdateAsync(UpdateOnePersonInput input, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetAsync(input.Id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail<PersonDto>("Person not found", HttpStatusCode.NotFound);
        }

        var phone = mapper.Map<Phone>(input.Phone);
        var address = mapper.Map<Address>(input.Address);

        var updatePerson = foundEntity.Update(input.Fullname, input.Document, input.PersonType, input.EmployeeRole, input.Email, input.Password, phone,
            address);
        foundEntity.Validate();
        var updatedEntity = await repository.UpdateAsync(updatePerson, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<PersonDto>(updatedEntity));
    }

    public async Task<Response<Paginate<PersonDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var response = await repository.GetAllAsync([nameof(Person.Vehicles), nameof(Person.Address)], paginatedRequest, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<PersonDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }
}
