using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Services.Interfaces;

public interface ISupplyService
{
    Task<Response<SupplyDto>> ChangeStock(Guid id, int quantity, bool adding, CancellationToken cancellationToken);
}
