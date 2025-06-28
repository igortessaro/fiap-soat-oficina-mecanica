using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentResults;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public class AvailableServiceService(
    IMapper mapper,
    IAvailableServiceRepository repository,
    ISupplyRepository supplyRepository,
    IAvailableServiceSupplyRepository availableServiceSupplyRepository) : IAvailableService
{
    public async Task<Result<AvailableServiceDto>> CreateAsync(CreateAvailableServiceRequest request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<AvailableService>(request);
        if (!request.Supplies.Any()) return Result.Ok(mapper.Map<AvailableServiceDto>(await repository.AddAsync(entity, cancellationToken)));

        foreach (var supply in request.Supplies)
        {
            var foundSupply = await supplyRepository.GetByIdAsync(supply, cancellationToken);
            if (foundSupply is null) return Result.Fail(new Error($"Supply with ID {supply} not found"));
            _ = entity.AddSupply(foundSupply);
        }

        var created = await repository.AddAsync(entity, cancellationToken);
        return Result.Ok(mapper.Map<AvailableServiceDto>(created));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var found = await repository.GetByIdAsync(id, cancellationToken);
        if (found is null) return Result.Fail(new Error("AvailableService not found"));
        await repository.DeleteAsync(found, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<Paginate<AvailableServiceDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync(paginatedRequest, cancellationToken);
        var mapped = mapper.Map<Paginate<AvailableServiceDto>>(result);
        return Result.Ok(mapped);
    }

    public async Task<Result<AvailableServiceDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var found = await repository.GetByIdAsync(id, cancellationToken);
        return found != null
            ? Result.Ok(mapper.Map<AvailableServiceDto>(found))
            : Result.Fail(new Error("AvailableService Not Found"));
    }

    public async Task<Result<AvailableServiceDto>> UpdateAsync(UpdateOneAvailableServiceInput input, CancellationToken cancellationToken)
    {
        var found = await repository.GetAsync(input.Id, cancellationToken);
        if (found is null) return Result.Fail(new Error("AvailableService not found"));
        await availableServiceSupplyRepository.DeleteRangeAsync(found.AvailableServiceSupplies, cancellationToken);
        foreach (var supply in input.Supplies)
        {
            var foundSupply = await supplyRepository.GetByIdAsync(supply, cancellationToken);
            if (foundSupply is null) return Result.Fail(new Error($"Supply with ID {supply} not found"));
            _ = found.AddSupply(foundSupply);
        }

        var updated = await repository.UpdateAsync(found.Update(input.Name, input.Price), cancellationToken);
        return Result.Ok(mapper.Map<AvailableServiceDto>(updated));
    }
}
