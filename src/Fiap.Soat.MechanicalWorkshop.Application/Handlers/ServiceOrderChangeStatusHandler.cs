using Fiap.Soat.MechanicalWorkshop.Application.Commands;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using MediatR;

namespace Fiap.Soat.MechanicalWorkshop.Application.Handlers;

public sealed class ServiceOrderChangeStatusHandler(IServiceOrderService service) : IRequestHandler<ServiceOrderChangeStatusCommand, Response<ServiceOrderDto>>
{
    public async Task<Response<ServiceOrderDto>> Handle(ServiceOrderChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var changingStatusResponse = await service.PatchAsync(new UpdateOneServiceOrderInput(request.Id, request.Status), cancellationToken);
        if (!changingStatusResponse.IsSuccess) return changingStatusResponse;

        return changingStatusResponse;
    }
}
