using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Domain.Interfaces;
using OrderAPI.Infrastructure.Repositories;

namespace OrderAPI.Infrastructure.DependencyInjection
{
    public static class RepositoryDependencyInjection
    {
        public static void AddRepositoryDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}
