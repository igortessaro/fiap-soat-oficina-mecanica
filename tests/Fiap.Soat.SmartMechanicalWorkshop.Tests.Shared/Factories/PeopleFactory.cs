using Bogus;
using Bogus.Extensions.Brazil;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;
using Person = Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities.Person;

namespace Fiap.Soat.SmartMechanicalWorkshop.Tests.Shared.Factories;

[ExcludeFromCodeCoverage]
public static class PeopleFactory
{
    public static Person CreateClient() => Create(PersonType.Client, null);

    public static Person CreateDetailerEmployee() => Create(PersonType.Employee, EmployeeRole.Detailer);

    private static Person Create(PersonType personType, EmployeeRole? employeeRole)
    {
        var faker = new Faker("pt_BR");
        return new Person(
            faker.Name.FullName(),
            faker.Person.Cpf(),
            personType,
            employeeRole,
            faker.Internet.Email(),
            faker.Internet.Password(),
            new Phone(faker.Phone.PhoneNumber()),
            new Address(faker.Address.StreetName(), faker.Address.City(), faker.Address.State(), faker.Address.ZipCode()));
    }
}
