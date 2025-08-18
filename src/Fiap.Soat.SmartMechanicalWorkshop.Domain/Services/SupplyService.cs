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
    public async Task<Response<SupplyDto>> ChangeStock(Guid id, int quantity, bool adding, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return ResponseFactory.Fail<SupplyDto>("Supply not found", HttpStatusCode.NotFound);
        await repository.UpdateAsync(adding ? entity.AddToStock(quantity) : entity.RemoveFromStock(quantity), cancellationToken);
        return ResponseFactory.Ok(mapper.Map<SupplyDto>(entity));
    }
}
