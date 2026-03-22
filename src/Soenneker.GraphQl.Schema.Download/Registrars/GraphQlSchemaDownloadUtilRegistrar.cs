using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.GraphQl.Schema.Download.Abstract;

namespace Soenneker.GraphQl.Schema.Download.Registrars;

/// <summary>
/// A GraphQL schema download utility
/// </summary>
public static class GraphQlSchemaDownloadUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IGraphQlSchemaDownloadUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddGraphQlSchemaDownloadUtilAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<IGraphQlSchemaDownloadUtil, GraphQlSchemaDownloadUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IGraphQlSchemaDownloadUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddGraphQlSchemaDownloadUtilAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<IGraphQlSchemaDownloadUtil, GraphQlSchemaDownloadUtil>();

        return services;
    }
}
