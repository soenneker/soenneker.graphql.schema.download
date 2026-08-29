using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.GraphQl.Schema.Download.Abstract;
using Soenneker.Utils.HttpClientCache.Registrar;

namespace Soenneker.GraphQl.Schema.Download.Registrars;

/// <summary>
/// A GraphQL schema download utility
/// </summary>
public static class GraphQlSchemaDownloadUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IGraphQlSchemaDownloadUtil"/> as a singleton service. <para/>
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
    /// Adds <see cref="IGraphQlSchemaDownloadUtil"/> as a scoped service. <para/>
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
