using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class AvailableServiceConfiguration : IEntityTypeConfiguration<AvailableService>
{
    public void Configure(EntityTypeBuilder<AvailableService> builder)
    {
        builder.ToTable("available_services");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.Property(s => s.Name)
            .HasColumnName("name")
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Price)
            .HasColumnName("price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.HasMany(x => x.Supplies)
            .WithMany(x => x.AvailableServices)
            .UsingEntity<Dictionary<string, object>>(
                "available_service_supplies", // Nome da tabela de junção
                j => j.HasOne<Supply>()
                      .WithMany()
                      .HasForeignKey("supply_id") // Nome da coluna FK para Supply
                      .HasConstraintName("fk_available_service_supplies_supply_id")
                      .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<AvailableService>()
                      .WithMany()
                      .HasForeignKey("available_service_id") // Nome da coluna FK para AvailableService
                      .HasConstraintName("fk_available_service_supplies_available_service_id")
                      .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("available_service_id", "supply_id");
                    j.ToTable("available_service_supplies");
                }
            );
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
