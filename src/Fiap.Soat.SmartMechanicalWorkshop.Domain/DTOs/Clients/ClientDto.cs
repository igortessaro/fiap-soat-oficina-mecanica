namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record ClientDto(Guid Id, string Fullname, string Document, PhoneDto Phone);

public record PhoneDto(string CountryCode, string AreaCode, string Number, string Type);
