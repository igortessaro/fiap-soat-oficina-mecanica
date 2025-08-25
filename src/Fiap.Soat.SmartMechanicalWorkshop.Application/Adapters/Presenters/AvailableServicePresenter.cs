using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using System.Net;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public static class AvailableServicePresenter
{
    public static Response<AvailableServiceDto> ToDto(AvailableService entity, HttpStatusCode httpStatusCode = HttpStatusCode.OK) =>
        ResponseFactory.Ok(new AvailableServiceDto(entity.Id, entity.Name, entity.Price, entity.AvailableServiceSupplies.Select(SupplyPresenter.ToDto).ToList()), httpStatusCode);
}
