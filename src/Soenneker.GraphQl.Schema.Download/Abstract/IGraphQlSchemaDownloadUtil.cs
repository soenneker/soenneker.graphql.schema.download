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
    /// <param name="endpoint">The GraphQL endpoint.</param>
    /// <param name="headers">Optional headers to include with the request.</param>
    /// <param name="bearerToken">An optional raw authentication token. When supplied, it is sent as an <c>Authorization: Bearer</c> header.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask<string> Download(string endpoint, IReadOnlyDictionary<string, string>? headers = null, string? bearerToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads the raw GraphQL introspection JSON payload by using the supplied <see cref="System.Net.Http.HttpClient"/>.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send the request.</param>
    /// <param name="endpoint">The GraphQL endpoint.</param>
    /// <param name="headers">Optional headers to include with the request.</param>
    /// <param name="bearerToken">An optional raw authentication token. When supplied, it is sent as an <c>Authorization: Bearer</c> header.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask<string> Download(System.Net.Http.HttpClient httpClient, string endpoint, IReadOnlyDictionary<string, string>? headers = null,
        string? bearerToken = null, CancellationToken cancellationToken = default);
}
