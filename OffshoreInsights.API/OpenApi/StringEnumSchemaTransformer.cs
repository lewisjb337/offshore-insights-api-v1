using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Text.Json.Nodes;

namespace OffshoreInsights.API.OpenApi;

/// <summary>
/// Transforms enum schemas from integer representations to named string values,
/// allowing Scalar to render them as a selectable dropdown rather than a free-text field.
/// </summary>
public class StringEnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo is null)
            return Task.CompletedTask;

        // Unwrap Nullable<T> so VesselType? is treated the same as VesselType
        var type = Nullable.GetUnderlyingType(context.JsonTypeInfo.Type) ?? context.JsonTypeInfo.Type;

        if (!type.IsEnum)
            return Task.CompletedTask;

        schema.Type = JsonSchemaType.String;
        schema.Format = null;
        schema.Enum = Enum.GetNames(type)
            .Select(name => (JsonNode)JsonValue.Create(name)!)
            .ToList();

        return Task.CompletedTask;
    }
}
