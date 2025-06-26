namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record UpdateOneClientRequest(string Fullname, string Document, string Email, UpdateOnePhoneRequest? Phone, UpdateOneAddressRequest? Address);

public record UpdateOnePhoneRequest(string CountryCode, string AreaCode, string Number, string Type);

public record UpdateOneAddressRequest(string Street, string City, string State, string ZipCode);
