using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record CreateClientRequest([Required] string Fullname, [Required] string Document);