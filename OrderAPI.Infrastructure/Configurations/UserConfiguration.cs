using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAPI.Domain.Entities;


namespace OrderAPI.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId);

            builder.Property(p => p.Username)
                   .IsRequired();

            builder.Property(p => p.PasswordHash)
                   .IsRequired();

            builder.Property(p => p.Username)
                   .IsRequired();

            builder.Property(p => p.CreatedAt)
                   .IsRequired();
        }
    }
}
