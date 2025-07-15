using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class EventLogConfiguration : IEntityTypeConfiguration<ServiceOrderEvent>
{
    public void Configure(EntityTypeBuilder<ServiceOrderEvent> builder)
    {
        builder.ToTable("service_order_events");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(x => x.ServiceOrderId).HasColumnName("service_order_id").IsRequired();
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(x => x.ServiceOrder)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
