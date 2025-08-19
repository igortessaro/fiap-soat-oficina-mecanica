using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.Update;

public record UpdateVehicleCommand(Guid Id, string LicensePlate, int? ManufactureYear, string Brand, string Model, Guid? PersonId) : IRequest<Response<VehicleDto>>;
