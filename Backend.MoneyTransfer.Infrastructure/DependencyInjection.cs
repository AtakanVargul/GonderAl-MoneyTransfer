using MassTransit;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Backend.MoneyTransfer.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Backend.MoneyTransfer.Infrastructure.Services;
using Backend.MoneyTransfer.Infrastructure.Persistence;
using Backend.MoneyTransfer.Infrastructure.Authorization;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using Backend.MoneyTransfer.Application.Common.Models.IdentityConfiguration;

namespace Backend.MoneyTransfer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IDomainEventService, DomainEventService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }

    public static void AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddPasswordValidator<PasswordValidatorService>()
            .AddDefaultTokenProviders();

        var identitySettings = new IdentitySettings();
        configuration.GetSection(nameof(IdentitySettings)).Bind(identitySettings);

        services.Configure<IdentityOptions>(opt =>
        {
            if (identitySettings.Password != null)
            {
                opt.Password.RequiredLength = identitySettings.Password.RequiredLength;
                opt.Password.RequireDigit = identitySettings.Password.RequireDigit;
                opt.Password.RequireLowercase = identitySettings.Password.RequireLowercase;
                opt.Password.RequireUppercase = identitySettings.Password.RequireUppercase;
                opt.Password.RequireNonAlphanumeric = identitySettings.Password.RequireNonAlphanumeric;
            }
            if (identitySettings.Lockout != null)
            {
                opt.Lockout.AllowedForNewUsers = identitySettings.Lockout.AllowedForNewUsers;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.Lockout.DefaultLockoutTimeSpanInMinutes);
                opt.Lockout.MaxFailedAccessAttempts = identitySettings.Lockout.MaxFailedAccessAttempts;
            }
            if (identitySettings.UserOptions != null)
            {
                opt.User.RequireUniqueEmail = identitySettings.UserOptions.RequireUniqueEmail;
            }
        });
    }

    public static void AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var identitySettings = new IdentitySettings();
        configuration.GetSection(nameof(IdentitySettings)).Bind(identitySettings);

        var signingKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(identitySettings.TokenSettings.TokenAuthenticationSettings.SecretKey));

        var tokenAuthenticationSettings = identitySettings.TokenSettings.TokenAuthenticationSettings;
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = tokenAuthenticationSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = tokenAuthenticationSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            TokenDecryptionKey = signingKey,
        };
        var tokenProviderOptions = new TokenProviderOptions()
        {
            Audience = tokenAuthenticationSettings.Audience,
            Issuer = tokenAuthenticationSettings.Issuer,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            IsOtpTokenPathEnabled = false,
            Expiration = TimeSpan.FromMinutes(identitySettings.TokenSettings.TokenExpiryDefaultMinute)
        };

        services.AddAuthentication(cfg =>
        {
            cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = tokenValidationParameters;
            options.Events = new JwtBearerEvents
            {
                OnChallenge = (context) =>
                {
                    context.HandleResponse();
                    var response = new UnauthorizedAccessException();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.WriteAsync(response.ToString()).Wait();

                    return Task.CompletedTask;
                }
            };
        });

        services.AddSingleton(tokenProviderOptions);
        services.AddTransient<IJwtHelper, JwtHelper>();
    }

    public static void AddMasstransitService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}