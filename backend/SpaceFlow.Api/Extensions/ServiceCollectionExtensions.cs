using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpaceFlow.Api.Data;
using SpaceFlow.Api.Infrastructure.Authentication;
using SpaceFlow.Api.Infrastructure.Cors;
using SpaceFlow.Api.Services;

namespace SpaceFlow.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiFoundation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();
        services.AddHealthChecks();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITestService, TestService>();

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();

        services.AddConfiguredCors(configuration);
        services.AddConfiguredAuthentication(configuration);

        return services;
    }

    private static IServiceCollection AddConfiguredCors(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration.GetSection(CorsOptions.SectionName).Get<CorsOptions>() ?? new CorsOptions();
        var origins = corsOptions.AllowedOrigins.Length > 0
            ? corsOptions.AllowedOrigins
            : ["http://localhost:4200"];

        services.AddCors(options =>
        {
            options.AddPolicy("AngularClient", policy =>
            {
                policy.WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

    private static IServiceCollection AddConfiguredAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        var signingKey = jwtOptions.Key;
        if (string.IsNullOrWhiteSpace(signingKey) || signingKey.Length < 32)
        {
            throw new InvalidOperationException("Jwt:Key must be configured and at least 32 characters long.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = securityKey,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("StaffAccess", policy => policy.RequireRole("Admin", "Staff"));
            options.AddPolicy("FinanceAccess", policy => policy.RequireRole("Admin", "Finance"));
        });

        return services;
    }
}