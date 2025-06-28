namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record UpdateOneClientRequest(string Fullname, string Document, string Email, UpdateOnePhoneRequest? Phone, UpdateOneAddressRequest? Address);

public record UpdateOnePhoneRequest(string AreaCode, string Number);

public record UpdateOneAddressRequest(string Street, string City, string State, string ZipCode);
