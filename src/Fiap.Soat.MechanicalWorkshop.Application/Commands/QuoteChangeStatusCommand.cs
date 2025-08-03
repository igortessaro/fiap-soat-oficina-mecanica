using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Commands;

public record QuoteChangeStatusCommand(Guid Id, QuoteStatus Status, Guid ServiceOrderId) : IRequest<Response<QuoteDto>>;
