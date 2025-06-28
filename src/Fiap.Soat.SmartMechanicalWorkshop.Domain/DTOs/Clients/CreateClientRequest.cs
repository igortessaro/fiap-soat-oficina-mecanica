using System.ComponentModel.DataAnnotations;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record CreateClientRequest([Required][MaxLength(255)] string Fullname, [Required][MaxLength(100)] string Document, [Required][MaxLength(255)] string Email, CreatePhoneRequest Phone, CreateAddressRequest Address);

public record CreatePhoneRequest(
    [Required][MaxLength(5)] string AreaCode,
    [Required][MaxLength(15)] string Number
);


public record CreateAddressRequest(
    [Required][MaxLength(100)] string Street,
    [Required][MaxLength(60)] string City,
    [Required][MaxLength(30)] string State,
    [Required][MaxLength(15)] string ZipCode
);
