using Bogus;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using Person = Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities.Person;

namespace Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;

public static class PeopleFactory
{
    public static Person CreateClient() => Create(EPersonType.Client, null);

    public static Person CreateDetailerEmployee() => Create(EPersonType.Employee, EEmployeeRole.Detailer);

    private static Person Create(EPersonType personType, EEmployeeRole? employeeRole)
    {
        var faker = new Faker("pt_BR");
        return new Person(
            faker.Name.FullName(),
            "",
            personType,
            employeeRole,
            faker.Internet.Email(),
            new Phone(faker.Phone.PhoneNumber()),
            new Address(faker.Address.StreetName(), faker.Address.City(), faker.Address.State(), faker.Address.ZipCode()));
    }
}
