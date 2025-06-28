using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Converters;

public class PhoneConverter() : ValueConverter<Phone, string>(phone => phone, phone => new Phone(phone));
