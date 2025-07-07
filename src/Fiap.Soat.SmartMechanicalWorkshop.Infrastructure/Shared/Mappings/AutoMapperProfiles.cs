using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.AvailableServices;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Clients;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.ServiceOrders;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.ValueObjects;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Shared.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Vehicle, VehicleDto>().ReverseMap();
        CreateMap<Paginate<Vehicle>, Paginate<VehicleDto>>().ReverseMap();
        CreateMap<Vehicle, CreateNewVehicleRequest>()
            .ReverseMap()
            .ConstructUsing(dest => new Vehicle(dest.Model, dest.Brand, dest.ManufactureYear, dest.LicensePlate, dest.ClientId));
        CreateMap<Vehicle, UpdateOneVehicleRequest>().ReverseMap();

        CreateMap<AvailableService, AvailableServiceDto>()
            .ReverseMap();
        CreateMap<Paginate<AvailableService>, Paginate<AvailableServiceDto>>().ReverseMap();
        CreateMap<AvailableService, CreateAvailableServiceRequest>()
            .ReverseMap();
        CreateMap<AvailableService, UpdateOneAvailableServiceRequest>().ReverseMap();

        CreateMap<Supply, SupplyDto>().ReverseMap();
        CreateMap<Paginate<Supply>, Paginate<SupplyDto>>().ReverseMap();
        CreateMap<Supply, CreateNewSupplyRequest>().ReverseMap();
        CreateMap<Supply, UpdateOneSupplyRequest>().ReverseMap();

        CreateMap<Client, ClientDto>().ReverseMap();
        CreateMap<Paginate<Client>, Paginate<ClientDto>>().ReverseMap();
        CreateMap<Client, CreateClientRequest>().ReverseMap();
        CreateMap<Client, UpdateOneClientRequest>().ReverseMap();

        CreateMap<Phone, PhoneDto>().ReverseMap();
        CreateMap<Phone, CreatePhoneRequest>().ReverseMap();
        CreateMap<Phone, UpdateOnePhoneInput>().ReverseMap();
        CreateMap<Phone, UpdateOnePhoneRequest>().ReverseMap();

        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Address, CreateAddressRequest>().ReverseMap();
        CreateMap<Address, UpdateOneAddressInput>().ReverseMap();
        CreateMap<Address, UpdateOneAddressRequest>().ReverseMap();

        CreateMap<Email, EmailDto>().ReverseMap();

        CreateMap<ServiceOrder, ServiceOrderDto>()

            .ReverseMap();
        CreateMap<Paginate<ServiceOrder>, Paginate<ServiceOrderDto>>().ReverseMap();
        CreateMap<ServiceOrder, CreateServiceOrderRequest>()
            .ReverseMap()
            .ConstructUsing(dest => new ServiceOrder(dest.Title, dest.Description, dest.VehicleId, dest.ClientId));
        CreateMap<ServiceOrder, UpdateOneServiceOrderRequest>().ReverseMap();
    }
}
