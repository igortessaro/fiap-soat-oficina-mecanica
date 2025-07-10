using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(a => a.Street)
            .HasColumnName("street")
            .HasColumnType("VARCHAR(100)");
        builder.Property(a => a.City)
            .HasColumnName("city")
            .HasColumnType("VARCHAR(60)");
        builder.Property(a => a.State)
            .HasColumnName("state")
            .HasColumnType("VARCHAR(30)");
        builder.Property(a => a.ZipCode)
            .HasColumnName("zip_code")
            .HasColumnType("VARCHAR(15)");

        builder.HasIndex(x => new { x.Street, x.City, x.State, x.ZipCode }).IsUnique();
    }
}
