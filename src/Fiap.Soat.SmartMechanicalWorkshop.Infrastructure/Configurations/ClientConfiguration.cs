using Fiap.Soat.SmartMechanicalWorkshop.Domain.Domains.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired();
        builder.Property(x => x.Fullname)
            .HasColumnName("fullname")
            .HasColumnType("VARCHAR(255)")
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(x => x.Document)
            .HasColumnName("document")
            .HasColumnType("VARCHAR(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(c => c.Phone, phone =>
        {
            phone.Property(p => p.CountryCode)
                .HasColumnName("phoneCountryCode")
                .HasColumnType("VARCHAR(5)");
            phone.Property(p => p.AreaCode)
                .HasColumnName("phoneAreaCode")
                .HasColumnType("VARCHAR(5)");
            phone.Property(p => p.Number)
                .HasColumnName("phoneNumber")
                .HasColumnType("VARCHAR(15)");
            phone.Property(p => p.Type)
                .HasColumnName("phoneType")
                .HasColumnType("VARCHAR(10)");
        });

        builder.OwnsOne(c => c.Address, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("addressStreet")
                .HasColumnType("VARCHAR(100)");
            address.Property(a => a.City)
                .HasColumnName("addressCity")
                .HasColumnType("VARCHAR(60)");
            address.Property(a => a.State)
                .HasColumnName("addressState")
                .HasColumnType("VARCHAR(30)");
            address.Property(a => a.ZipCode)
                .HasColumnName("addressZipCode")
                .HasColumnType("VARCHAR(15)");
        });

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(a => a.Address)
                .HasColumnName("email")
                .HasColumnType("VARCHAR(255)");
        });
    }
}
