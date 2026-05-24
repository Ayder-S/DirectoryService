using DS.Application.Abstractions;
using DS.Contracts.Locations.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DS.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
        
        var assembly = typeof(DependencyInjectionExtensions).Assembly;

        services.Scan(scan => scan.FromAssemblies(assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return services;
    }
}