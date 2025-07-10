using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("vehicles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.Property(v => v.LicensePlate)
            .HasColumnName("license_plate")
            .HasColumnType("VARCHAR(20)")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(v => v.Model)
            .HasColumnName("model")
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Brand)
            .HasColumnName("brand")
            .HasColumnType("VARCHAR(50)")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.ManufactureYear)
            .HasColumnName("manufacture_year")
            .HasColumnType("INT")
            .IsRequired();

        builder.Property(v => v.ClientId)
            .HasColumnName("client_id")
            .IsRequired();

        builder.HasOne(v => v.Person)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.LicensePlate).IsUnique();
    }
}
