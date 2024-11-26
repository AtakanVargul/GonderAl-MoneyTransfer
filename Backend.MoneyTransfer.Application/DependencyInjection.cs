using Backend.MoneyTransfer.Application.Common.Behaviours;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Backend.MoneyTransfer.Application.Common.Models.Audit;
using Backend.MoneyTransfer.Application.Common.Interfaces;
using MassTransit;

namespace Backend.MoneyTransfer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));


        //services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
        services.AddScoped<IAuditLogService, AuditLogService>();

        return services;
    }
}