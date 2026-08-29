[![](https://img.shields.io/nuget/v/soenneker.graphql.schema.download.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.graphql.schema.download/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.graphql.schema.download/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.graphql.schema.download/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.graphql.schema.download.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.graphql.schema.download/)

# Soenneker.GraphQl.Schema.Download

A GraphQL schema download utility.

## Install

```bash
dotnet add package Soenneker.GraphQl.Schema.Download
```

## Quick start

```csharp
using Soenneker.GraphQl.Schema.Download.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddGraphQlSchemaDownloadUtilAsSingleton();
```

Adds `IGraphQlSchemaDownloadUtil` as a singleton service.

## What you get

- `IGraphQlSchemaDownloadUtil` — A GraphQL schema download utility.
- `GraphQlSchemaDownloadUtilRegistrar` — A GraphQL schema download utility.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IGraphQlSchemaDownloadUtil.Download(endpoint, headers, bearerToken, cancellationToken)` | Downloads the raw GraphQL introspection JSON payload from the specified endpoint. | A task whose result is the text returned by download. |
| `IGraphQlSchemaDownloadUtil.Download(httpClient, endpoint, headers, bearerToken, cancellationToken)` | Downloads the raw GraphQL introspection JSON payload by using the supplied `System.Net.Http.HttpClient`. | A task whose result is the text returned by download. |
| `GraphQlSchemaDownloadUtilRegistrar.AddGraphQlSchemaDownloadUtilAsSingleton(services)` | Adds `IGraphQlSchemaDownloadUtil` as a singleton service. | The same service collection, so additional registrations can be chained. |
| `GraphQlSchemaDownloadUtilRegistrar.AddGraphQlSchemaDownloadUtilAsScoped(services)` | Adds `IGraphQlSchemaDownloadUtil` as a scoped service. | The same service collection, so additional registrations can be chained. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
