using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Text.Json.Nodes;

namespace OffshoreInsights.API.OpenApi;

/// <summary>
/// Stamps example values on every path parameter in the OpenAPI spec.
/// Without these, Scalar blocks the request with "Path parameters must have values".
/// </summary>
public class PathParameterExamplesTransformer : IOpenApiOperationTransformer
{
    /// <summary>
    /// Example values keyed by the lowercase parameter name.
    /// Long values are used for MMSI-style identifiers; 1 is used for generic ID params.
    /// </summary>
    private static readonly Dictionary<string, JsonNode> Examples = new(StringComparer.OrdinalIgnoreCase)
    {
        ["mmsi"] = JsonValue.Create(123456789L)!,
        ["id"]   = JsonValue.Create(1L)!,
    };

    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (operation.Parameters is null)
            return Task.CompletedTask;

        foreach (var parameter in operation.Parameters.OfType<OpenApiParameter>()
                     .Where(p => p.In == ParameterLocation.Path))
        {
            if (Examples.TryGetValue(parameter.Name, out var example))
                parameter.Example = example;
        }

        return Task.CompletedTask;
    }
}
