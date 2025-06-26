using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

public class PaginatedRequest
{
    [Required]
    [DefaultValue(1)]
    public int PageNumber { get; set; } = 1;
    [Required]
    [DefaultValue(15)]
    public int PageSize { get; set; } = 15;
}