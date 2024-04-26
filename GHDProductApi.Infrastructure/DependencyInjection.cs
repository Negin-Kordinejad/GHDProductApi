using GHDProductApi.Infrastructure.Contexts;
using GHDProductApi.Infrastructure.Intefaces;
using GHDProductApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GHDProductApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<ProductDbContext>();

            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
