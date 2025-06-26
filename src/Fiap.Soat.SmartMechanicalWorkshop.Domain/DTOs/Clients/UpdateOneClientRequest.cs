namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record UpdateOneClientRequest(string Fullname, string Document, UpdateOnePhoneRequest? Phone);

public record UpdateOnePhoneRequest(string CountryCode, string AreaCode, string Number, string Type);