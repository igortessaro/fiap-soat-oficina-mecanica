using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Person;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Shared.Mappings;

[ExcludeFromCodeCoverage]
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Vehicle, VehicleDto>().ReverseMap();
        CreateMap<Paginate<Vehicle>, Paginate<VehicleDto>>().ReverseMap();
        CreateMap<Vehicle, CreateNewVehicleRequest>()
            .ReverseMap()
            .ConstructUsing(dest => new Vehicle(dest.Model, dest.Brand, dest.ManufactureYear, dest.LicensePlate, dest.PersonId));
        CreateMap<Vehicle, UpdateOneVehicleRequest>().ReverseMap();

        CreateMap<AvailableService, AvailableServiceDto>()
            .ConstructUsing(dest => new AvailableServiceDto(dest.Id, dest.Name, dest.Price, dest.AvailableServiceSupplies.Select(x => new SupplyDto(x.SupplyId, x.Supply.Name, x.Quantity, x.Supply.Price)).ToList()))
            .ReverseMap();
        CreateMap<Paginate<AvailableService>, Paginate<AvailableServiceDto>>().ReverseMap();
        CreateMap<AvailableService, CreateAvailableServiceRequest>()
            .ReverseMap();
        CreateMap<AvailableService, UpdateOneAvailableServiceRequest>().ReverseMap();

        CreateMap<Supply, SupplyDto>().ReverseMap();
        CreateMap<Paginate<Supply>, Paginate<SupplyDto>>().ReverseMap();
        CreateMap<Supply, CreateNewSupplyRequest>().ReverseMap();
        CreateMap<Supply, UpdateOneSupplyRequest>().ReverseMap();

        CreateMap<Person, PersonDto>().ReverseMap();
        CreateMap<Paginate<Person>, Paginate<PersonDto>>().ReverseMap();
        CreateMap<Person, CreatePersonRequest>().ReverseMap();
        CreateMap<Person, UpdateOnePersonRequest>().ReverseMap();

        CreateMap<Phone, PhoneDto>().ReverseMap();
        CreateMap<Phone, CreatePhoneRequest>().ReverseMap();
        CreateMap<Phone, UpdateOnePhoneInput>().ReverseMap();
        CreateMap<Phone, UpdateOnePhoneRequest>().ReverseMap();

        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Address, CreateAddressRequest>().ReverseMap();
        CreateMap<Address, UpdateOneAddressInput>().ReverseMap();
        CreateMap<Address, UpdateOneAddressRequest>().ReverseMap();

        CreateMap<Email, EmailDto>().ReverseMap();

        CreateMap<ServiceOrder, ServiceOrderDto>().ReverseMap();
        CreateMap<Paginate<ServiceOrder>, Paginate<ServiceOrderDto>>().ReverseMap();
        CreateMap<ServiceOrder, CreateServiceOrderRequest>()
            .ReverseMap()
            .ConstructUsing(dest => new ServiceOrder(dest.Title, dest.Description, dest.VehicleId, dest.ClientId));
        CreateMap<ServiceOrder, UpdateOneServiceOrderRequest>().ReverseMap();

        CreateMap<Quote, QuoteDto>().ReverseMap();
        CreateMap<ServiceOrderEvent, ServiceOrderEventDto>().ReverseMap();

        CreateMap<AvailableServiceSupply, SupplyDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SupplyId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Supply.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Supply.Price));
    }
}
