using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Ports.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.List;

public sealed class ListVehiclesHandler(IMapper mapper, IVehicleRepository vehicleRepository) : IRequestHandler<ListVehiclesQuery, Response<Paginate<VehicleDto>>>
{
    public async Task<Response<Paginate<VehicleDto>>> Handle(ListVehiclesQuery request, CancellationToken cancellationToken)
    {
        var response = await vehicleRepository.GetAllAsync(request, cancellationToken);
        var mappedResponse = mapper.Map<Paginate<VehicleDto>>(response);
        return ResponseFactory.Ok(mappedResponse);
    }
}
