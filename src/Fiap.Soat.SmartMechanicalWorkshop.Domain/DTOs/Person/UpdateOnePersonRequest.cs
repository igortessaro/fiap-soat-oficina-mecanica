using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;

public record UpdateOnePersonRequest(string Fullname, string Document, EPersonType PersonType, EEmployeeRole? EmployeeRole, string Email, string Password, UpdateOnePhoneRequest? Phone, UpdateOneAddressRequest? Address);

public record UpdateOnePhoneRequest(string AreaCode, string Number);

public record UpdateOneAddressRequest(string Street, string City, string State, string ZipCode);
