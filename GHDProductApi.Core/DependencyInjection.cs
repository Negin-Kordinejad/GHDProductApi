using FluentValidation;
using GHDProductApi.Core.Common.Behaviours;
using GHDProductApi.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GHDProductApi.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddInfrastructureServices();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

            return services;
        }
    }
}
