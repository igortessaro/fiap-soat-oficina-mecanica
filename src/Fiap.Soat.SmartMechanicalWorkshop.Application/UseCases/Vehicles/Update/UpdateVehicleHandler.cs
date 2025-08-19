using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Update;

public sealed class UpdateVehicleHandler(IMapper mapper, IVehicleRepository repository) : IRequestHandler<UpdateVehicleCommand, Response<VehicleDto>>
{
    public async Task<Response<VehicleDto>> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var foundEntity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (foundEntity is null)
        {
            return ResponseFactory.Fail<VehicleDto>("Vehicle not found", HttpStatusCode.NotFound);
        }

        _ = foundEntity.Update(request.ManufactureYear, request.LicensePlate, request.Brand, request.Model);
        if (!foundEntity.LicensePlate.IsValid())
        {
            return ResponseFactory.Fail<VehicleDto>("Invalid license plate format");
        }

        var updatedEntity = await repository.UpdateAsync(foundEntity, cancellationToken);
        return ResponseFactory.Ok(mapper.Map<VehicleDto>(updatedEntity));
    }
}
