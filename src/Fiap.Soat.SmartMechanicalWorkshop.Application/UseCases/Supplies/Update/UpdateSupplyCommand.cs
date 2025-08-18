using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.UseCases.Supplies.Update;

public record UpdateSupplyCommand(Guid Id, string Name, int? Quantity, decimal? Price) : IRequest<Response<SupplyDto>>;
