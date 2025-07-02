using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
public record SendServiceOrderApprovalRequest([Required] Guid Id)
{
}
