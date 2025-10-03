using Microsoft.Extensions.DependencyInjection;
using OrderAPI.Application.Interfaces;
using OrderAPI.Application.Services;

namespace OrderAPI.Application.DependencyInjection
{
    public static class ServiceDependencyInjection
    {
        public static void AddServiceDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
