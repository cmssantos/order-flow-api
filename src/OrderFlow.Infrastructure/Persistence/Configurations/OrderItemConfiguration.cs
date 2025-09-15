using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain;
using OrderFlow.Domain.Entities;

namespace OrderFlow.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.HasIndex(oi => new { oi.OrderId, oi.ProductId }).IsUnique();

        builder.Property<Guid>("OrderId");
        builder.HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey("OrderId");

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        builder.OwnsOne(oi => oi.Quantity, q =>
        {
            q.Property(p => p.Value)
                .HasColumnName("Quantity")
                .IsRequired();
        });

        builder.OwnsOne(oi => oi.UnitPrice, up =>
        {
            up.Property(p => p.Value)
                .HasColumnName("UnitPrice")
                .IsRequired()
                .HasPrecision(
                    FieldLengths.ProductPriceValuePrecision,
                    FieldLengths.ProductPriceValueScale
                );
        });

        builder.Ignore(o => o.Total);
    }
}
