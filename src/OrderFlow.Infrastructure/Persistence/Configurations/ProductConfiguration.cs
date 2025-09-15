using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Persistence.Configurations;

public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(c => c.Id);

        builder.OwnsOne(c => c.Name, name =>
        {
            name.Property(n => n.Value)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(FieldLengths.ProductNameMaxLength);
        });

        builder.OwnsOne(c => c.Sku, sku =>
        {
            sku.Property(e => e.Value)
                .HasColumnName("Sku")
                .IsRequired()
                .HasMaxLength(FieldLengths.ProductSkuMaxLength);

            sku.HasIndex(s => s.Value).IsUnique();
        });

        builder.OwnsOne(c => c.Description, description =>
        {
            description.Property(e => e.Value)
                .HasColumnName("Description")
                .IsRequired()
                .HasMaxLength(FieldLengths.ProductDescriptionMaxLength);
        });

        builder.OwnsOne(c => c.Price, price =>
        {
            price.Property(e => e.Value)
                .HasColumnName("Price")
                .IsRequired()
                .HasPrecision(
                    FieldLengths.ProductPriceValuePrecision,
                    FieldLengths.ProductPriceValueScale
                );
        });

        builder.OwnsOne(c => c.Stock, stock =>
        {
            stock.Property(p => p.Value)
                .HasColumnName("Stock")
                .IsRequired();
        });
    }
}
