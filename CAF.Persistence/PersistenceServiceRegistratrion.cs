using System.Reflection;
using CAF.Application.Abstractions.Services.Authentication;
using CAF.Application.Repositories;
using CAF.Domain.Entities.Authentication;
using CAF.Persistence.Contexts;
using CAF.Persistence.Repositories;
using CAF.Persistence.Services.Authentication;
using CAF.Persistence.Services.KronosStationModule;
using CAF.Persistence.Services.KronosStationModule.Job;
using Kronos.StationModule.Contexts;
using Kronos.StationModule.Services;
using Microsoft.AspNetCore.Identity;
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
            options.UseNpgsql(configuration.GetConnectionString("PostgreSql"));
            options.EnableSensitiveDataLogging();
        });

        #region Identity Configuration
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 3;
            options.User.RequireUniqueEmail = false;
            options.SignIn.RequireConfirmedEmail = false;
        })
           .AddEntityFrameworkStores<CAFDbContext>()
           .AddDefaultTokenProviders();
        #endregion

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

       

        services.AddScoped<IInternalAuthentication, AuthService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        #region Kronos 

        services.AddDbContext<AutomationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Automation")); // 1
            options.EnableSensitiveDataLogging();
        });
        services.AddDbContext<AlpetDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Alpet")); // 2
            options.EnableSensitiveDataLogging();
        });
        services.AddDbContext<AytemizDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Aytemiz")); // 3
            options.EnableSensitiveDataLogging();
        });

        services.AddDbContext<BpDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("BP")); // 4
            options.EnableSensitiveDataLogging();
        });

        services.AddDbContext<GGGDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("GGG")); // 5
            options.EnableSensitiveDataLogging();
        });

        services.AddDbContext<GorpetDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Gorpet")); // 6
            options.EnableSensitiveDataLogging();
        });

        services.AddDbContext<ModogluDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Modoglu")); // 7
            options.EnableSensitiveDataLogging();
        });

        services.AddDbContext<ShellDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Shell")); // 8
            options.EnableSensitiveDataLogging();
        });

        services.AddDbContext<TascoDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Tasco")); // 9
            options.EnableSensitiveDataLogging();
        });

        services.AddDbContext<TotalDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Total")); // 10
            options.EnableSensitiveDataLogging();
        });

        services.AddDbContext<TpDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("TP")); // 11
            options.EnableSensitiveDataLogging();
        });



        services.AddScoped<IStationService, StationService>();


        #region Jobs 
        services.AddHostedService<StationBackgroundSyncJob>();
        #endregion

        #endregion

        return services;
    }

}
