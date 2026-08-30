using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GraphQl.Schema.Download.Abstract;

/// <summary>
/// Downloads and validates GraphQL introspection responses.
/// </summary>
public interface IGraphQlSchemaDownloadUtil
{
    /// <summary>
    /// Posts an introspection query to the specified endpoint and returns its raw JSON response.
    /// </summary>
    /// <param name="endpoint">The GraphQL HTTP endpoint.</param>
    /// <param name="headers">Optional request or content headers.</param>
    /// <param name="bearerToken">An optional raw authentication token. When supplied, it is sent as an <c>Authorization: Bearer</c> header.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The validated introspection response exactly as returned by the endpoint.</returns>
    ValueTask<string> Download(string endpoint, IReadOnlyDictionary<string, string>? headers = null, string? bearerToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Posts an introspection query with the supplied <see cref="System.Net.Http.HttpClient"/> and returns its raw JSON response.
    /// </summary>
    /// <param name="httpClient">The caller-owned client used to send the request. It is not disposed by this method.</param>
    /// <param name="endpoint">The GraphQL HTTP endpoint.</param>
    /// <param name="headers">Optional request or content headers.</param>
    /// <param name="bearerToken">An optional raw authentication token. When supplied, it is sent as an <c>Authorization: Bearer</c> header.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The validated introspection response exactly as returned by the endpoint.</returns>
    ValueTask<string> Download(System.Net.Http.HttpClient httpClient, string endpoint, IReadOnlyDictionary<string, string>? headers = null,
        string? bearerToken = null, CancellationToken cancellationToken = default);
}
