using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using FluentResults;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public class AvailableServiceService(IAvailableServiceRepository repository, IMapper mapper) : IAvailableService
{
    public async Task<Result<AvailableServiceDto>> CreateAsync(CreateAvailableServiceRequest request,
        CancellationToken cancellationToken)
    {
        AvailableService entity = mapper.Map<AvailableService>(request);
        AvailableService? created = await repository.AddAsync(entity, cancellationToken);
        return created != null
            ? Result.Ok(mapper.Map<AvailableServiceDto>(created))
            : Result.Fail(new Error("Not Created"));
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        AvailableService found = await repository.GetByIdAsync(id, cancellationToken);
        if (found == null)
        {
            return Result.Fail(new Error("AvailableService not found"));
        }

        await repository.DeleteAsync(found, cancellationToken);
        return Result.Ok();
    }

    public async Task<Result<Paginate<AvailableServiceDto>>> GetAllAsync(PaginatedRequest paginatedRequest,
        CancellationToken cancellationToken)
    {
        Paginate<AvailableService> result = await repository.GetAllAsync(paginatedRequest, cancellationToken);
        Paginate<AvailableServiceDto> mapped = mapper.Map<Paginate<AvailableServiceDto>>(result);
        return Result.Ok(mapped);
    }

    public async Task<Result<AvailableServiceDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        AvailableService? found = await repository.GetByIdAsync(id, cancellationToken);
        return found != null
            ? Result.Ok(mapper.Map<AvailableServiceDto>(found))
            : Result.Fail(new Error("AvailableService Not Found"));
    }

    public async Task<Result<AvailableServiceDto>> UpdateAsync(UpdateOneAvailableServiceInput input,
        CancellationToken cancellationToken)
    {
        AvailableService found = await repository.GetByIdAsync(input.Id, cancellationToken);
        if (found == null)
        {
            return Result.Fail(new Error("AvailableService not found"));
        }

        if (input.Name != null)
        {
            found.Name = input.Name;
        }

        if (input.Price != null)
        {
            found.Price = (decimal) input.Price;
        }

        AvailableService? updated = await repository.UpdateAsync(found, cancellationToken);
        return updated != null
            ? Result.Ok(mapper.Map<AvailableServiceDto>(updated))
            : Result.Fail(new Error("Not Updated"));
    }
}
