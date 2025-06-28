using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class ClientService(IMapper mapper, IClientRepository repository) : IClientService
{
    public async Task<Response<ClientDto>> CreateAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
        var mapperEntity = mapper.Map<Client>(request);
        var createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
        return Response<ClientDto>.Ok(mapper.Map<ClientDto>(createdEntity), System.Net.HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        if (foundEntity is null)
        {
            return Response.Fail(new FluentResults.Error("Client not found"), System.Net.HttpStatusCode.NotFound);
        }

        await repository.DeleteAsync(foundEntity, cancellationToken);
        return Response.Ok();
    }

    public async Task<Response<ClientDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        return foundEntity != null
            ? Response<ClientDto>.Ok(mapper.Map<ClientDto>(foundEntity))
            : Response<ClientDto>.Fail(new FluentResults.Error("Client Not Found"), System.Net.HttpStatusCode.NotFound);
    }

    public async Task<Response<ClientDto>> UpdateAsync(UpdateOneClientInput input, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetAsync(input.Id, cancellationToken);
        if (foundEntity is null)
        {
            return Response<ClientDto>.Fail(new FluentResults.Error("Client not found"), System.Net.HttpStatusCode.NotFound);
        }

        var phone = mapper.Map<Phone>(input.Phone);
        var address = mapper.Map<Address>(input.Address);
        var updatedEntity = await repository.UpdateAsync(foundEntity.Update(input.Fullname, input.Document, input.Email, phone, address), cancellationToken);
        return Response<ClientDto>.Ok(mapper.Map<ClientDto>(updatedEntity));
    }

    public async Task<Response<Paginate<ClientDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var response = await repository.GetAllAsync(paginatedRequest, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<ClientDto>>(response);
        return Response<Paginate<ClientDto>>.Ok(mappedResponse);
    }
}
