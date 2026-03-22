using System.Text.Json.Serialization;

namespace Soenneker.GraphQl.Schema.Download.Dtos;

internal sealed record IntrospectionPayload(
    [property: JsonPropertyName("query")] string Query,
    [property: JsonPropertyName("operationName")]
    string OperationName)
{
    public static readonly IntrospectionPayload Instance = new(GraphQlSchemaDownloadConstants.IntrospectionQuery, "IntrospectionQuery");
}