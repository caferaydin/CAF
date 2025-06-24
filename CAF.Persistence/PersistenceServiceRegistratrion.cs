using System.Reflection;
using CAF.Application.Abstractions.Services.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CAF.Persistence;

public static class PersistenceServiceRegistratrion
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your persistence services here
        // Example: services.AddScoped<IUserService, UserService>();

        services.AddDbContext<Contexts.CAFDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging();
        });

        #region IoC Reflection
        var applicationAssembly = Assembly.Load("CAF.Application");
        var persistenceAssembly = Assembly.Load("CAF.Persistence");


        var repositoryInterfaces = applicationAssembly.GetTypes()
           .Where(t => t.IsInterface && t.Name.EndsWith("Repository")).ToList();

        var repositoryClasses = persistenceAssembly.GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Repository")).ToList();

        foreach (var repositoryInterface in repositoryInterfaces)
        {
            var repositoryClass = repositoryClasses.FirstOrDefault(c => c.GetInterfaces().Contains(repositoryInterface));
            if (repositoryClass != null)
            {
                services.AddScoped(repositoryInterface, repositoryClass);
            }
        }

        var serviceInterfaces = applicationAssembly.GetTypes()
           .Where(t => t.IsInterface && t.Name.EndsWith("Service")).ToList();

        var serviceClasses = persistenceAssembly.GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Service")).ToList();

        foreach (var serviceInterface in serviceInterfaces)
        {
            var serviceClass = serviceClasses.FirstOrDefault(c => c.GetInterfaces().Contains(serviceInterface));
            if (serviceClass != null)
            {
                services.AddScoped(serviceInterface, serviceClass);
            }
        }

        #endregion

        services.AddScoped<IInternalAuthentication, IAuthService>();

        return services;
    }

}
