using AutoRepairShopManagementSystem.Data;
using AutoRepairShopManagementSystem.Domains.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Domain.Repositories.Interfaces;

namespace AutoRepairShopManagementSystem.Repositories
{
    public class VehicleRepository(AppDbContext appDbContext) : Repository<Vehicle>(appDbContext), IVehicleRepository
    {
    }
}
