using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GraphQl.Schema.Download.Abstract;

/// <summary>
/// A GraphQL schema download utility
/// </summary>
public interface IGraphQlSchemaDownloadUtil
{
    /// <summary>
    /// Downloads the raw GraphQL introspection JSON payload from the specified endpoint.
    /// </summary>
    ValueTask<string> Download(string endpoint, IReadOnlyDictionary<string, string>? headers = null, string? bearerToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads the raw GraphQL introspection JSON payload by using the supplied <see cref="System.Net.Http.HttpClient"/>.
    /// </summary>
    ValueTask<string> Download(System.Net.Http.HttpClient httpClient, string endpoint, IReadOnlyDictionary<string, string>? headers = null,
        string? bearerToken = null, CancellationToken cancellationToken = default);
}
