namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record ClientDto(Guid Id, string Fullname, string Document, PhoneDto Phone, EmailDto Email, AddressDto Address);

public record PhoneDto(string CountryCode, string AreaCode, string Number, string Type);

public record EmailDto(string Address);

public record AddressDto(string Street, string City, string State, string ZipCode);