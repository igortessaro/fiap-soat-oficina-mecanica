using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using System.Net;

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
            return ResponseFactory.Fail<AvailableServiceDto>($"AvailableService with name {request.Name} already exists", HttpStatusCode.Conflict);
        }

        if (!request.Supplies.Any())
        {
            return ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(await repository.AddAsync(entity, cancellationToken)), HttpStatusCode.Created);
        }

        foreach (var supply in request.Supplies)
        {
            var foundSupply = await supplyRepository.GetByIdAsync(supply.SupplyId, cancellationToken);
            if (foundSupply is null)
            {
                return ResponseFactory.Fail<AvailableServiceDto>($"Supply with ID {supply} not found", HttpStatusCode.NotFound);
            }

            _ = entity.AddSupply(supply.SupplyId, supply.Quantity);
        }

        var created = await repository.AddAsync(entity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(created), HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var found = await repository.GetByIdAsync(id, cancellationToken);
        if (found is null)
        {
            return ResponseFactory.Fail("AvailableService not found", HttpStatusCode.NotFound);
        }

        await repository.DeleteAsync(found, cancellationToken);
        return ResponseFactory.Ok(HttpStatusCode.NoContent);
    }

    public async Task<Response<Paginate<AvailableServiceDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync(["AvailableServiceSupplies.Supply"], paginatedRequest, cancellationToken);
        var mapped = mapper.Map<Paginate<AvailableServiceDto>>(result);
        return ResponseFactory.Ok(mapped);
    }

    public async Task<Response<AvailableServiceDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var found = await repository.GetAsync(id, cancellationToken);
        return found != null
            ? ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(found))
            : ResponseFactory.Fail<AvailableServiceDto>("AvailableService Not Found", HttpStatusCode.NotFound);
    }

    public async Task<Response<AvailableServiceDto>> UpdateAsync(UpdateOneAvailableServiceInput input, CancellationToken cancellationToken)
    {
        var found = await repository.GetAsync(input.Id, cancellationToken);
        if (found is null)
        {
            return ResponseFactory.Fail<AvailableServiceDto>("AvailableService not found", HttpStatusCode.NotFound);
        }

        if (!input.Name.Equals(found.Name, StringComparison.CurrentCultureIgnoreCase) &&
            await repository.AnyAsync(x => input.Name.ToLower().Equals(x.Name.ToLower()), cancellationToken))
        {
            return ResponseFactory.Fail<AvailableServiceDto>($"AvailableService with name {input.Name} already exists", HttpStatusCode.Conflict);
        }

        foreach (var supply in input.Supplies)
        {
            var foundSupply = await supplyRepository.GetByIdAsync(supply.SupplyId, cancellationToken);
            if (foundSupply is null)
            {
                return ResponseFactory.Fail<AvailableServiceDto>($"Supply with ID {supply} not found", HttpStatusCode.NotFound);
            }
        }

        var supplies = input.Supplies.Select(x => new ServiceSupplyDto(input.Id, x.SupplyId, x.Quantity)).ToList();
        var updated = await repository.UpdateAsync(input.Id, input.Name, input.Price, supplies, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<AvailableServiceDto>(updated));
    }
}
