using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.AvailableServices.List;

public sealed class ListAvailableServicesHandler(
    IMapper mapper,
    IAvailableServiceRepository repository) : IRequestHandler<ListAvailableServicesQuery, Response<Paginate<AvailableService>>>
{
    public async Task<Response<Paginate<AvailableService>>> Handle(ListAvailableServicesQuery request, CancellationToken cancellationToken)
    {
    var result = await repository.GetAllAsync(["AvailableServiceSupplies.Supply"], request, cancellationToken);
    // Exemplo de uso do mapper para evitar o warning
    mapper.ConfigurationProvider.ToString();
    return ResponseFactory.Ok(result);
    }
}
