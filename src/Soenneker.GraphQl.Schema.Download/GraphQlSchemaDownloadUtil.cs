using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.ValueTask;
using Soenneker.GraphQl.Schema.Download.Abstract;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Extensions.String;
using Soenneker.Extensions.Task;
using Soenneker.GraphQl.Schema.Download.Dtos;

namespace Soenneker.GraphQl.Schema.Download;

/// <inheritdoc cref="IGraphQlSchemaDownloadUtil"/>
public sealed class GraphQlSchemaDownloadUtil : IGraphQlSchemaDownloadUtil
{
    private readonly IHttpClientCache _httpClientCache;

    public GraphQlSchemaDownloadUtil(IHttpClientCache httpClientCache)
    {
        _httpClientCache = httpClientCache;
    }

    public async ValueTask<string> Download(string endpoint, IReadOnlyDictionary<string, string>? headers = null, string? bearerToken = null,
        CancellationToken cancellationToken = default)
    {
        HttpClient cachedHttpClient = await _httpClientCache.Get(nameof(GraphQlSchemaDownloadUtil), cancellationToken)
                                                            .NoSync();

        return await DownloadInternal(cachedHttpClient, endpoint, headers, bearerToken, ownsClient: false, cancellationToken)
            .NoSync();
    }

    public ValueTask<string> Download(HttpClient httpClient, string endpoint, IReadOnlyDictionary<string, string>? headers = null, string? bearerToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient);

        return DownloadInternal(httpClient, endpoint, headers, bearerToken, ownsClient: false, cancellationToken);
    }

    private static async ValueTask<string> DownloadInternal(HttpClient httpClient, string endpoint, IReadOnlyDictionary<string, string>? headers,
        string? bearerToken, bool ownsClient, CancellationToken cancellationToken)
    {
        if (endpoint.IsNullOrWhiteSpace())
            throw new ArgumentException("A GraphQL endpoint is required.", nameof(endpoint));

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            request.Content = new StringContent(JsonSerializer.Serialize(IntrospectionPayload.Instance), Encoding.UTF8, "application/json");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (bearerToken.HasContent())
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            if (headers is not null)
            {
                foreach ((string key, string value) in headers)
                {
                    if (key.IsNullOrWhiteSpace())
                        continue;

                    if (!request.Headers.TryAddWithoutValidation(key, value))
                        request.Content.Headers.TryAddWithoutValidation(key, value);
                }
            }

            using HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken)
                                                                 .NoSync();
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync(cancellationToken)
                                        .NoSync();

            if (json.IsNullOrWhiteSpace())
                throw new InvalidOperationException("The GraphQL endpoint returned an empty response.");

            ValidateIntrospectionResponse(json);

            return json;
        }
        finally
        {
            if (ownsClient)
                httpClient.Dispose();
        }
    }

    private static void ValidateIntrospectionResponse(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;

        if (root.TryGetProperty("errors", out JsonElement errors) && errors.ValueKind == JsonValueKind.Array && errors.GetArrayLength() > 0)
            throw new InvalidOperationException($"The GraphQL endpoint returned introspection errors: {GetErrorMessage(errors)}");

        if (!TryGetSchema(root, out _))
            throw new InvalidOperationException("The GraphQL response did not contain a valid introspection schema payload.");
    }

    private static bool TryGetSchema(JsonElement root, out JsonElement schema)
    {
        if (root.ValueKind == JsonValueKind.Object)
        {
            if (root.TryGetProperty("data", out JsonElement data) && data.ValueKind == JsonValueKind.Object && data.TryGetProperty("__schema", out schema))
            {
                return true;
            }

            if (root.TryGetProperty("__schema", out schema))
                return true;

            if (root.TryGetProperty("types", out _))
            {
                schema = root;
                return true;
            }
        }

        schema = default;
        return false;
    }

    private static string GetErrorMessage(JsonElement errors)
    {
        var builder = new StringBuilder();

        foreach (JsonElement error in errors.EnumerateArray())
        {
            if (builder.Length > 0)
                builder.Append(" | ");

            if (error.ValueKind == JsonValueKind.Object && error.TryGetProperty("message", out JsonElement message) &&
                message.ValueKind == JsonValueKind.String)
            {
                builder.Append(message.GetString());
            }
            else
            {
                builder.Append(error.GetRawText());
            }
        }

        return builder.Length == 0 ? "Unknown GraphQL error." : builder.ToString();
    }
}