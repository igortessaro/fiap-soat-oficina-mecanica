using AutoMapper;
using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.DTOs.Supplies;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Shared.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
             CreateMap<Vehicle, VehicleDto>().ReverseMap();
             CreateMap<Paginate<Vehicle>, Paginate<VehicleDto>>().ReverseMap();
             CreateMap<Vehicle, CreateNewVehicleRequest>().ReverseMap();
             CreateMap<Vehicle, UpdateOneVehicleRequest>().ReverseMap();

            CreateMap<Supply, SupplyDto>().ReverseMap();
            CreateMap<Paginate<Supply>, Paginate<SupplyDto>>().ReverseMap();
            CreateMap<Supply, CreateNewSupplyRequest>().ReverseMap();
            CreateMap<Supply, UpdateOneSupplyRequest>().ReverseMap();

        }
    }
}
