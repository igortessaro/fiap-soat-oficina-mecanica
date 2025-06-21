using AutoMapper;
using AutoRepairShopManagementSystem.Domains.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Api.DTOs.Vehicles;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Shared;

namespace Fiap.Soat.SmartMechanicalWorkshop.Api.Shared.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
             CreateMap<Vehicle, VehicleDto>().ReverseMap();
             CreateMap<Paginate<Vehicle>, Paginate<VehicleDto>>().ReverseMap();
             CreateMap<Vehicle, CreateNewVehicleRequest>().ReverseMap();
             CreateMap<Vehicle, UpdateOneVehicleRequest>().ReverseMap();


        }
    }
}
