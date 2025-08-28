using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.List;

public sealed class ListVehiclesHandler(IVehicleRepository vehicleRepository) : IRequestHandler<ListVehiclesQuery, Response<Paginate<Vehicle>>>
{
    public async Task<Response<Paginate<Vehicle>>> Handle(ListVehiclesQuery request, CancellationToken cancellationToken)
    {
        var response = await vehicleRepository.GetAllAsync(request, cancellationToken);
        return ResponseFactory.Ok(response);
    }
}
