namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record UpdateOneClientInput(Guid Id, string Fullname, string Document, UpdateOnePhoneInput? Phone);

public record UpdateOnePhoneInput(string CountryCode, string AreaCode, string Number, string Type);
