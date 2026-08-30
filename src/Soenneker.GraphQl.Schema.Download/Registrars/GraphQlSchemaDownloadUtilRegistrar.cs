using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.GraphQl.Schema.Download.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.GraphQl.Schema.Download.Registrars;

/// <summary>
/// Registers the GraphQL schema downloader and its shared HTTP client cache.
/// </summary>
public static class GraphQlSchemaDownloadUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IGraphQlSchemaDownloadUtil"/> as a singleton service.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddGraphQlSchemaDownloadUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddSingleton<IGraphQlSchemaDownloadUtil, GraphQlSchemaDownloadUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IGraphQlSchemaDownloadUtil"/> as a scoped service while retaining the HTTP client cache as a singleton.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddGraphQlSchemaDownloadUtilAsScoped(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton()
                .TryAddScoped<IGraphQlSchemaDownloadUtil, GraphQlSchemaDownloadUtil>();

        return services;
    }
}
