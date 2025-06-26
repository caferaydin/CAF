using System.Reflection;
using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CAF.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            typeAdapterConfig.Scan(Assembly.Load("CAF.Application"));
            services.AddSingleton(typeAdapterConfig);
            // Register other application services here
            services.AddScoped<IMapper, ServiceMapper>();

            services.AddCustomFluentValidation();
            return services;
        }

        public static IServiceCollection AddCustomFluentValidation(this IServiceCollection services)
        {
            var applicationAssembly = Assembly.Load("CAF.Application");

            services.AddFluentValidationAutoValidation(); // Otomatik çalıştırma
            services.AddFluentValidationClientsideAdapters(); // İstersen

            return services;
        }
    }
}
