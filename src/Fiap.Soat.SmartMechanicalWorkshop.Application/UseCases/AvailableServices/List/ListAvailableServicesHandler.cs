using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.List;

public sealed class ListAvailableServicesHandler(
    IMapper mapper,
    IAvailableServiceRepository repository) : IRequestHandler<ListAvailableServicesQuery, Response<Paginate<AvailableServiceDto>>>
{
    public async Task<Response<Paginate<AvailableServiceDto>>> Handle(ListAvailableServicesQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync(["AvailableServiceSupplies.Supply"], request, cancellationToken);
        var mapped = mapper.Map<Paginate<AvailableServiceDto>>(result);
        return ResponseFactory.Ok(mapped);
    }
}
