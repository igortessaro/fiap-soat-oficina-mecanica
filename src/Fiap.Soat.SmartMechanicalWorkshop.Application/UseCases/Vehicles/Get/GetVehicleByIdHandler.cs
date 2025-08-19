using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Get;

public sealed class GetVehicleByIdHandler(IMapper mapper, IVehicleRepository vehicleRepository) : IRequestHandler<GetVehicleByIdQuery, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await vehicleRepository.GetByIdAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(mapper.Map<VehicleDto>(entity))
            : ResponseFactory.Fail<VehicleDto>("Vehicle Not Found", HttpStatusCode.NotFound);
    }
}
