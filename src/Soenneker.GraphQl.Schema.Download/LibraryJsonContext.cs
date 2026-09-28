using System.Text.Json.Serialization;
using Soenneker.GraphQl.Schema.Download.Dtos;

namespace Soenneker.GraphQl.Schema.Download;

[JsonSerializable(typeof(IntrospectionPayload))]
internal partial class LibraryJsonContext : JsonSerializerContext;
