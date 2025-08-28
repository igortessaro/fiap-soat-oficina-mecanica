using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Gateways.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Get;

public sealed class GetVehicleByIdHandler(IVehicleRepository vehicleRepository) : IRequestHandler<GetVehicleByIdQuery, Response<Vehicle>>
{
    public async Task<Response<Vehicle>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await vehicleRepository.GetByIdAsync(request.Id, cancellationToken);
        return entity != null
            ? ResponseFactory.Ok(entity)
            : ResponseFactory.Fail<Vehicle>("Vehicle Not Found", HttpStatusCode.NotFound);
    }
}
