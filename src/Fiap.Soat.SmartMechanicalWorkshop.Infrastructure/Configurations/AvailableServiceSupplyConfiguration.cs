using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class AvailableServiceSupplyConfiguration : IEntityTypeConfiguration<AvailableServiceSupply>
{
    public void Configure(EntityTypeBuilder<AvailableServiceSupply> builder)
    {
        builder.ToTable("available_service_supplies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(x => x.AvailableServiceId)
            .HasColumnName("available_service_id")
            .HasColumnType("CHAR(36)")
            .IsRequired();
        builder.Property(x => x.SupplyId)
            .HasColumnName("supply_id")
            .HasColumnType("CHAR(36)")
            .IsRequired();

        builder.HasOne(x => x.AvailableService)
            .WithMany(x => x.AvailableServiceSupplies)
            .HasForeignKey(x => x.AvailableServiceId);

        builder.HasOne(x => x.Supply)
            .WithMany(x => x.AvailableServiceSupplies)
            .HasForeignKey(x => x.SupplyId);

        builder.HasIndex(x => new { x.AvailableServiceId, x.SupplyId }).IsUnique();
    }
}
