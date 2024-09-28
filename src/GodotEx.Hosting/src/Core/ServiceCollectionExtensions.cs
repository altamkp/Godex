using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GodotEx.Hosting;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Adds a service as a singleton and hosted service.
    /// </summary>
    /// <typeparam name="TService">Type of service to add.</typeparam>
    /// <param name="services">Service collection instance.</param>
    /// <returns>Service collection instance.</returns>
    public static IServiceCollection AddSingletonHostedService<TService>(this IServiceCollection services)
           where TService : class, IHostedService {
        services.AddSingleton<TService>();
        services.AddHostedService(sp => sp.GetRequiredService<TService>());
        return services;
    }

    /// <summary>
    /// Adds a service as a singleton and hosted service.
    /// </summary>
    /// <typeparam name="TService">Type of service to add.</typeparam>
    /// <param name="services">Service collection instance.</param>
    /// <param name="implementationFactory">Implementation factory method for the service.</param>
    /// <returns>Service collection instance.</returns>
    public static IServiceCollection AddSingletonHostedService<TService>(
            this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
            where TService : class, IHostedService {
        services.AddSingleton(implementationFactory);
        services.AddHostedService(sp => sp.GetRequiredService<TService>());
        return services;
    }

    /// <summary>
    /// Adds a service as a singleton and hosted service.
    /// </summary>
    /// <typeparam name="TService">Type of service to add.</typeparam>
    /// <typeparam name="TImplementation">Type of implementation to add.</typeparam>
    /// <param name="services">Service collection instance.</param>
    /// <returns>Service collection instance.</returns>
    public static IServiceCollection AddSingletonHostedService<TService, TImplementation>(this IServiceCollection services)
           where TService : class, IHostedService
           where TImplementation : class, TService {
        services.AddSingleton<TService, TImplementation>();
        services.AddHostedService(sp => sp.GetRequiredService<TService>());
        return services;
    }

    /// <summary>
    /// Adds a service as a singleton and hosted service.
    /// </summary>
    /// <typeparam name="TService">Type of service to add.</typeparam>
    /// <typeparam name="TImplementation">Type of implementation to add.</typeparam>
    /// <param name="services">Service collection instance.</param>
    /// <param name="implementationFactory">Implementation factory method for the service.</param>
    /// <returns>Service collection instance.</returns>
    public static IServiceCollection AddSingletonHostedService<TService, TImplementation>(
            this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class, IHostedService
            where TImplementation : class, TService {
        services.AddSingleton<TService, TImplementation>(implementationFactory);
        services.AddHostedService(sp => sp.GetRequiredService<TService>());
        return services;
    }
}
