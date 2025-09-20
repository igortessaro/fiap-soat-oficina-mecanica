using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;

namespace Fiap.Soat.SmartMechanicalWorkshop.Application.Adapters.Presenters;

public static class QuotePresenter
{
    public static QuoteDto ToDto(Quote entity) => new QuoteDto(entity.Id, entity.Total, entity.Status, entity.ServiceOrderId);
}
