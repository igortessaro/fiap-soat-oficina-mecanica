using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Supply> Supplies { get; set; }
    public DbSet<AvailableService> AvailableServices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        modelBuilder.ApplyConfiguration(new AvailableServiceConfiguration());
        modelBuilder.ApplyConfiguration(new SupplyConfiguration());
        modelBuilder.ApplyConfiguration(new VehicleConfiguration());
    }
}
