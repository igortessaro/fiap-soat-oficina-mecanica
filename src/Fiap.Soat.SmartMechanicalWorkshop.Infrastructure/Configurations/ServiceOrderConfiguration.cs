using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class ServiceOrderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.ToTable("service_orders");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(x => x.ClientId).HasColumnName("client_id").IsRequired();
        builder.Property(x => x.VehicleId).HasColumnName("vehicle_id").IsRequired();
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("VARCHAR(500)")
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .HasColumnType("VARCHAR(250)")
            .HasMaxLength(250)
            .IsRequired();

        // builder.HasOne(x => x.Client)
        //     .WithMany(c => c.ServiceOrders)
        //     .HasForeignKey(x => x.ClientId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // builder.HasOne(x => x.Vehicle)
        //     .WithMany(v => v.ServiceOrders)
        //     .HasForeignKey(x => x.VehicleId)
        //     .OnDelete(DeleteBehavior.Cascade);

        // builder.HasMany(so => so.AvailableServices)
        //     .WithMany(a => a.ServiceOrders)
        //     .UsingEntity<Dictionary<string, object>>(
        //         "service_order_available_services",
        //         j => j.HasOne<AvailableService>()
        //             .WithMany()
        //             .HasForeignKey("available_service_id")
        //             .HasConstraintName("fk_service_order_available_services_available_service_id"),
        //         j => j.HasOne<ServiceOrder>()
        //             .WithMany()
        //             .HasForeignKey("service_order_id")
        //             .HasConstraintName("fk_service_order_available_services_service_order_id"),
        //         j =>
        //         {
        //             j.Property<Guid>("service_order_id").HasColumnName("service_order_id");
        //             j.Property<Guid>("available_service_id").HasColumnName("available_service_id");
        //             j.HasKey("service_order_id", "available_service_id");
        //         }
        //     );
    }
}
