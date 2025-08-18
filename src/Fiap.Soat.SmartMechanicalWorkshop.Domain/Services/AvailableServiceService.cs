using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services;

public sealed class AvailableServiceService(IMapper mapper, IAvailableServiceRepository repository) : IAvailableService
{
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
}
