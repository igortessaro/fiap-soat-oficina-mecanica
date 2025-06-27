namespace Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;

public record UpdateOneClientInput(Guid Id, string Fullname, string Document, string Email, UpdateOnePhoneInput? Phone, UpdateOneAddressInput? Address);

public record UpdateOnePhoneInput(string CountryCode, string AreaCode, string Number, string Type)
{
    public static implicit operator UpdateOnePhoneInput?(UpdateOnePhoneRequest? input)
    {
        return input is null ? null : new UpdateOnePhoneInput(input.CountryCode, input.AreaCode, input.Number, input.Type);
    }
}

public record UpdateOneAddressInput(string Street, string City, string State, string ZipCode)
{
    public static implicit operator UpdateOneAddressInput?(UpdateOneAddressRequest? input)
    {
        return input is null ? null : new UpdateOneAddressInput(input.Street, input.City, input.State, input.ZipCode);
    }
}
