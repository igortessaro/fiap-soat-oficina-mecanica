using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Vehicles.List;

public sealed class ListVehiclesQuery : PaginatedRequest, IRequest<Response<Paginate<VehicleDto>>>;
