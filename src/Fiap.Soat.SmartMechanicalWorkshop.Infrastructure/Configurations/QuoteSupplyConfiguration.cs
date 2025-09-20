using Fiap.Soat.SmartMechanicalWorkshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Configurations;

public sealed class QuoteSupplyConfiguration : IEntityTypeConfiguration<QuoteSupply>
{
    public void Configure(EntityTypeBuilder<QuoteSupply> builder)
    {
        builder.ToTable("quote_supplies");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(x => x.QuoteId).HasColumnName("quote_id").IsRequired();
        builder.Property(x => x.SupplyId).HasColumnName("supply_id").IsRequired();
        builder.Property(x => x.Price).HasColumnName("price").HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.Quantity).HasColumnName("quantity").IsRequired();

        builder.HasOne(x => x.Quote)
            .WithMany(x => x.Supplies)
            .HasForeignKey(x => x.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Supply)
            .WithMany()
            .HasForeignKey(x => x.SupplyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
