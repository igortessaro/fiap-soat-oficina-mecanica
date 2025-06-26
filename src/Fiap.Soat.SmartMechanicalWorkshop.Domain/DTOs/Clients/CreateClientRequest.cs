using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record CreateClientRequest([Required][MaxLength(255)] string Fullname, [Required][MaxLength(100)] string Document, CreatePhoneRequest Phone);

public record CreatePhoneRequest(
    [Required][MaxLength(5)] string CountryCode,
    [Required][MaxLength(5)] string AreaCode,
    [Required][MaxLength(15)] string Number,
    [Required][MaxLength(10)] string Type
);