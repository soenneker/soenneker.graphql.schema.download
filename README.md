[![](https://img.shields.io/nuget/v/soenneker.graphql.schema.download.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.graphql.schema.download/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.graphql.schema.download/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.graphql.schema.download/actions/workflows/publish-package.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.graphql.schema.download/build-and-test.yml?style=for-the-badge&label=build)](https://github.com/soenneker/soenneker.graphql.schema.download/actions/workflows/build-and-test.yml)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.graphql.schema.download/codeql.yml?style=for-the-badge&label=codeql)](https://github.com/soenneker/soenneker.graphql.schema.download/actions/workflows/codeql.yml)
[![](https://img.shields.io/nuget/dt/soenneker.graphql.schema.download.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.graphql.schema.download/)

# Soenneker.GraphQl.Schema.Download

Downloads a GraphQL server's introspection response and verifies that it contains a usable schema payload. The returned JSON can be archived as-is or passed to an introspection-to-SDL converter.

## Installation

```bash
dotnet add package Soenneker.GraphQl.Schema.Download
```

## Registration

```csharp
using Microsoft.Extensions.DependencyInjection;
using Soenneker.GraphQl.Schema.Download.Abstract;
using Soenneker.GraphQl.Schema.Download.Registrars;

services.AddGraphQlSchemaDownloadUtilAsScoped();

IGraphQlSchemaDownloadUtil downloader =
    serviceProvider.GetRequiredService<IGraphQlSchemaDownloadUtil>();
```

Scoped registration keeps the utility disposable with its consuming scope while the underlying HTTP client cache remains a singleton. Singleton utility registration is also available with `AddGraphQlSchemaDownloadUtilAsSingleton()`.

## Download a schema

```csharp
string introspectionJson = await downloader.Download(
    "https://api.example.com/graphql",
    bearerToken: accessToken,
    cancellationToken: cancellationToken);

await File.WriteAllTextAsync("introspection.json", introspectionJson, cancellationToken);
```

Additional request headers can be supplied when an API uses tenant, version, or API-key headers:

```csharp
var headers = new Dictionary<string, string>
{
    ["X-Api-Key"] = apiKey,
    ["X-Tenant-Id"] = tenantId
};

string introspectionJson = await downloader.Download(endpoint, headers, cancellationToken: cancellationToken);
```

To use an already configured client, pass it to the overload. The client remains owned by the caller and is not disposed by the downloader:

```csharp
string introspectionJson = await downloader.Download(httpClient, endpoint, cancellationToken: cancellationToken);
```

The request is an HTTP `POST` with the standard introspection query and an `application/json` body. Non-success HTTP responses throw `HttpRequestException`; malformed JSON throws `JsonException`; GraphQL errors, empty responses, and responses without a schema `types` array throw `InvalidOperationException`.
