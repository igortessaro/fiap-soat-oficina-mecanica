using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class ServiceOrderAvailableServiceConfiguration : IEntityTypeConfiguration<ServiceOrderAvailableService>
{
    public void Configure(EntityTypeBuilder<ServiceOrderAvailableService> builder)
    {
        builder.ToTable("service_order_available_services");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(x => x.AvailableServiceId)
            .HasColumnName("available_service_id")
            .HasColumnType("CHAR(36)")
            .IsRequired();
        builder.Property(x => x.ServiceOrderId)
            .HasColumnName("service_order_id")
            .HasColumnType("CHAR(36)")
            .IsRequired();

        builder.HasOne(x => x.ServiceOrder)
            .WithMany(x => x.ServiceOrderAvailableServices)
            .HasForeignKey(x => x.ServiceOrderId);

        builder.HasOne(x => x.AvailableService)
            .WithMany(x => x.ServiceOrderAvailableServices)
            .HasForeignKey(x => x.AvailableServiceId);

        builder.HasIndex(x => new { x.AvailableServiceId, x.ServiceOrderId }).IsUnique();
    }
}
