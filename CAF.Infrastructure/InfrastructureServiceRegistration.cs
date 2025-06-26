using System.Text;
using CAF.Application.Abstractions.Services.ExternalService;
using CAF.Application.Abstractions.Services.Session;
using CAF.Application.Abstractions.Token;
using CAF.Application.Models.Settings;
using CAF.Infrastructure.Services.Common;
using CAF.Infrastructure.Services.Session;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TokenHandler = CAF.Infrastructure.Services.Token.TokenHandler;

namespace CAF.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<ICurrentUserSession, CurrentUserSession>();
            services.AddScoped<IMailService, MailService>();

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            var jwtSettings = configuration.GetSection("Token").Get<JWTSettings>();

            services.AddAuthentication(IdentityConstants.ApplicationScheme) // ✅ Bu: "Identity.Application"
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtSettings = configuration.GetSection("Token").Get<JWTSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "text/plain";
                            return context.Response.WriteAsync("Authentication failed.");
                        }
                    };
                });
            // Authorization policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });


            return services;
        }
    }
}
