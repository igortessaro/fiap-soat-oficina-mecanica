using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class SupplyService(ISupplyRepository repository, IMapper mapper) : ISupplyService
{
    public async Task<Response<SupplyDto>> CreateAsync(CreateNewSupplyRequest request, CancellationToken cancellationToken)
    {
        if (await repository.AnyAsync(x => request.Name.ToLower().Equals(x.Name.ToLower()), cancellationToken))
        {
            return ResponseFactory.Fail<SupplyDto>($"Supply with name {request.Name} already exists", HttpStatusCode.Conflict);
        }

        var mapperEntity = mapper.Map<Supply>(request);
        var createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<SupplyDto>(createdEntity), HttpStatusCode.Created);
    }

    public async Task<Response> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail("Supply not found", HttpStatusCode.NotFound);
        }

        await repository.DeleteAsync(foundEntity, cancellationToken);
        return ResponseFactory.Ok(HttpStatusCode.NoContent);
    }

    public async Task<Response<Paginate<SupplyDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
    {
        var response = await repository.GetAllAsync(paginatedRequest, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<SupplyDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }

    public async Task<Response<SupplyDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        return foundEntity != null
            ? ResponseFactory.Ok(mapper.Map<SupplyDto>(foundEntity))
            : ResponseFactory.Fail<SupplyDto>("Supply Not Found", HttpStatusCode.NotFound);
    }

    public async Task<Response<SupplyDto>> UpdateAsync(UpdateOneSupplyInput input, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(input.Id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail<SupplyDto>("Supply not found", HttpStatusCode.NotFound);
        }

        var updatedEntity = await repository.UpdateAsync(foundEntity.Update(input.Name, input.Price, input.Quantity), cancellationToken);
        return ResponseFactory.Ok(mapper.Map<SupplyDto>(updatedEntity));
    }
}
