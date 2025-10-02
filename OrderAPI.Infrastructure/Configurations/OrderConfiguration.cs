using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAPI.Domain.Entities;


namespace OrderAPI.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.OrderId);

            builder.Property(p => p.OrderNumber)
                   .IsRequired();

            builder.Property(p => p.OrderTime)
                   .IsRequired();

            builder.Property(p => p.DeliveredInd)
                   .IsRequired();

            builder.HasMany(o => o.Occurrences)
                .WithOne()
                .HasForeignKey("OrderId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
