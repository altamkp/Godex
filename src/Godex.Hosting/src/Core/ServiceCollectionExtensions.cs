using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Godex.Hosting;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// Registers <typeparamref name="T"/> as a singleton <see cref="IHostedService"/>.
    /// </summary>
    /// <typeparam name="T">Hosted service to register.</typeparam>
    /// <param name="services">Service collection to add the hosted service to.</param>
    /// <returns><paramref name="services"/> for chaining.</returns>
    public static IServiceCollection AddSingletonHostedService<T>(this IServiceCollection services)
        where T : class, IHostedService {
        return services.AddSingleton<IHostedService, T>();
    }
}
