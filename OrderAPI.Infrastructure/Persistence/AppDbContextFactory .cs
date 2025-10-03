using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderAPI.Infrastructure.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // IMPORTANTE: Use a MESMA connection string do seu appsettings.json
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=OrderApiDb;Trusted_Connection=true;TrustServerCertificate=true"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}