using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class AvailableServiceSupplyConfiguration : IEntityTypeConfiguration<AvailableServiceSupply>
{
    public void Configure(EntityTypeBuilder<AvailableServiceSupply> builder)
    {
        builder.ToTable("available_services_supply");
        builder.HasKey(x => new { x.AvailableServiceId, x.SupplyId });
        builder.Property(x => x.AvailableServiceId)
            .HasColumnName("available_service_id")
            .HasColumnType("char(36)")
            .IsRequired();
        builder.Property(x => x.SupplyId)
            .HasColumnName("supply_id")
            .HasColumnType("char(36)")
            .IsRequired();

        builder.Property(s => s.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.HasOne(x => x.Supply)
            .WithMany(x => x.AvailableServiceSupplies)
            .HasForeignKey(x => x.SupplyId)
            .HasConstraintName("fk_available_service_supplies_supply_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.AvailableService)
            .WithMany(x => x.AvailableServiceSupplies)
            .HasForeignKey(x => x.AvailableServiceId)
            .HasConstraintName("fk_available_service_supplies_available_service_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
