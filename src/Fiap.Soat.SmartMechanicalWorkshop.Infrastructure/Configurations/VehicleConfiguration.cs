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

        builder.OwnsOne(c => c.LicensePlate, lp =>
        {
            lp.Property(a => a.Value)
                .HasColumnName("license_plate")
                .HasColumnType("VARCHAR(20)");

            lp.HasIndex(a => a.Value).IsUnique();
        });

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

        builder.Property(v => v.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.HasOne(v => v.Person)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
