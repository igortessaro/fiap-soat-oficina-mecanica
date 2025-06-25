using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Api.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Repositories;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Services
{
    public class SupplyService(SupplyRepository repository, IMapper mapper) : ISupplyService
    {
        public async Task<Result<SupplyDto>> CreateAsync(CreateNewSupplyRequest request, CancellationToken cancellationToken)
        {
            Supply mapperEntity = mapper.Map<Supply>(request);
            Supply? createdEntity = await repository.AddAsync(mapperEntity, cancellationToken);
            return createdEntity != null
                ? Result<SupplyDto>.Ok(mapper.Map<SupplyDto>(createdEntity), HttpStatusCode.Created)
                : Result<SupplyDto>.Fail(new FluentResults.Error("Not Created"), HttpStatusCode.InternalServerError);
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            Supply foundEntity = await repository.GetByIdAsync(id, cancellationToken);

            if (foundEntity == null)
            {
                return Result.Fail(new FluentResults.Error("Supply not found"), HttpStatusCode.NotFound);
            }

            await repository.DeleteAsync(foundEntity, cancellationToken);

            return Result.Ok();
        }

        public async Task<Result<Paginate<SupplyDto>>> GetAllAsync(PaginatedRequest paginatedRequest, CancellationToken cancellationToken)
        {
            Paginate<Supply> response = await repository.GetAllAsync(paginatedRequest, cancellationToken);
            Paginate<SupplyDto> mappedResponse = mapper.Map<Paginate<SupplyDto>>(response);
            return Result<Paginate<SupplyDto>>.Ok(mappedResponse);
        }

        public async Task<Result<SupplyDto>> GetOneAsync(Guid id, CancellationToken cancellationToken)
        {
            Supply? foundEntity = await repository.GetByIdAsync(id, cancellationToken);
            return foundEntity != null
                ? Result<SupplyDto>.Ok(mapper.Map<SupplyDto>(foundEntity))
                : Result<SupplyDto>.Fail(new FluentResults.Error("Supply Not Found"), HttpStatusCode.NotFound);
        }

        public async Task<Result<SupplyDto>> UpdateAsync(UpdateOneSupplyInput input, CancellationToken cancellationToken)
        {
            Supply foundEntity = await repository.GetByIdAsync(input.Id, cancellationToken);

            if (foundEntity == null)
            {
                return Result<SupplyDto>.Fail(new FluentResults.Error("Supply not found"), HttpStatusCode.NotFound);
            }

            if (input.Name != null)
            {
                foundEntity.Name = input.Name;
            }

            if (input.Price != null)
            {
                foundEntity.Price = input.Price.Value;
            }

            if (input.Quantity != null)
            {
                foundEntity.Quantity = input.Quantity.Value;
            }

            Supply? updatedEntity = await repository.UpdateAsync(foundEntity, cancellationToken);

            return updatedEntity != null
                ? Result<SupplyDto>.Ok(mapper.Map<SupplyDto>(updatedEntity))
                : Result<SupplyDto>.Fail(new FluentResults.Error("Not Updated"));
        }
    }
}
