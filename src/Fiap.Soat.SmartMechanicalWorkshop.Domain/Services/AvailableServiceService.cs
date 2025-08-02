using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentResults;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class AvailableServiceService(
    IMapper mapper,
    IAvailableServiceRepository repository,
    ISupplyRepository supplyRepository
) : IAvailableService
{
    public async Task<Response<AvailableServiceDto>> CreateAsync(CreateAvailableServiceRequest request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<AvailableService>(request);
        if (await repository.AnyAsync(x => request.Name.ToLower().Equals(x.Name.ToLower()), cancellationToken))
        {
            return ResponseFactory.Fail<AvailableServiceDto>(new Error($"AvailableService with name {request.Name} already exists"), System.Net.HttpStatusCode.Conflict);
        }

        if (!request.SuppliesIds.Any())
        {
            return ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(await repository.AddAsync(entity, cancellationToken)), System.Net.HttpStatusCode.Created);
        }

        foreach (var supply in request.SuppliesIds)
        {
            var foundSupply = await supplyRepository.GetByIdAsync(supply, cancellationToken);
            if (foundSupply is null) return ResponseFactory.Fail<AvailableServiceDto>(new Error($"Supply with ID {supply} not found"), System.Net.HttpStatusCode.NotFound);
            _ = entity.AddSupply(foundSupply);
        }

        var created = await repository.AddAsync(entity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(created), System.Net.HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var found = await repository.GetByIdAsync(id, cancellationToken);
        if (found is null) return ResponseFactory.Fail(new Error("AvailableService not found"), System.Net.HttpStatusCode.NotFound);
        await repository.DeleteAsync(found, cancellationToken);
        return ResponseFactory.Ok(System.Net.HttpStatusCode.NoContent);
    }

    public async Task<Response<Paginate<AvailableServiceDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync(paginatedRequest, cancellationToken);
        var mapped = mapper.Map<Paginate<AvailableServiceDto>>(result);
        return ResponseFactory.Ok(mapped);
    }

    public async Task<Response<AvailableServiceDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var found = await repository.GetAsync(id, cancellationToken);
        return found != null
            ? ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(found))
            : ResponseFactory.Fail<AvailableServiceDto>(new Error("AvailableService Not Found"), System.Net.HttpStatusCode.NotFound);
    }

    public async Task<Response<AvailableServiceDto>> UpdateAsync(UpdateOneAvailableServiceInput input, CancellationToken cancellationToken)
    {
        var found = await repository.GetAsync(input.Id, cancellationToken);
        if (found is null) return ResponseFactory.Fail<AvailableServiceDto>(new Error("AvailableService not found"), System.Net.HttpStatusCode.NotFound);
        if (!input.Name.Equals(found.Name, StringComparison.CurrentCultureIgnoreCase) &&
            await repository.AnyAsync(x => input.Name.ToLower().Equals(x.Name.ToLower()), cancellationToken))
        {
            return ResponseFactory.Fail<AvailableServiceDto>(new Error($"AvailableService with name {input.Name} already exists"), System.Net.HttpStatusCode.Conflict);
        }

        var supplies = new List<Supply>();
        foreach (var supply in input.SuppliesIds ?? [])
        {
            var foundSupply = await supplyRepository.GetByIdAsync(supply, cancellationToken);
            if (foundSupply is null)
            {
                return ResponseFactory.Fail<AvailableServiceDto>(new Error($"Supply with ID {supply} not found"), System.Net.HttpStatusCode.NotFound);
            }

            supplies.Add(foundSupply);
        }

        var updated = await repository.UpdateAsync(input.Id, input.Name, input.Price, supplies, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(updated));
    }
}
