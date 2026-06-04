using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Text.Json.Nodes;

namespace OffshoreInsights.API.OpenApi;

/// <summary>
/// Adds named bounding box examples to the vessel positions endpoint, giving consumers
/// preset coordinate ranges for common offshore regions to select from in Scalar.
/// </summary>
public class VesselPositionExamplesTransformer : IOpenApiOperationTransformer
{
    private static readonly Dictionary<string, RegionBounds> Regions = new()
    {
        ["northSea"]       = new("North Sea",       51.0,  61.0,  -4.0, 10.0),
        ["balticSea"]      = new("Baltic Sea",       53.0,  66.0,  10.0, 30.0),
        ["englishChannel"] = new("English Channel",  49.5,  51.5,  -5.5,  2.5),
        ["norwegianSea"]   = new("Norwegian Sea",    62.0,  72.0, -10.0, 15.0),
        ["skagerrak"]      = new("Skagerrak",        55.0,  59.0,   7.0, 13.0),
    };

    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (!IsVesselPositionsEndpoint(context))
            return Task.CompletedTask;

        foreach (var parameter in operation.Parameters?.OfType<OpenApiParameter>() ?? [])
        {
            Func<RegionBounds, double>? selector = parameter.Name switch
            {
                "MinLatitude"  => r => r.MinLatitude,
                "MaxLatitude"  => r => r.MaxLatitude,
                "MinLongitude" => r => r.MinLongitude,
                "MaxLongitude" => r => r.MaxLongitude,
                _ => null
            };

            if (selector is null) continue;

            parameter.Examples = Regions.ToDictionary(
                kvp => kvp.Key,
                kvp => (IOpenApiExample)new OpenApiExample
                {
                    Summary = kvp.Value.DisplayName,
                    Value   = JsonValue.Create(selector(kvp.Value))
                });
        }

        return Task.CompletedTask;
    }

    private static bool IsVesselPositionsEndpoint(OpenApiOperationTransformerContext context) =>
        context.Description.ActionDescriptor is ControllerActionDescriptor descriptor
        && descriptor.ControllerName == "Vessels"
        && context.Description.ActionDescriptor.RouteValues.TryGetValue("action", out var action)
        && action == "GetVesselPositions";

    private record RegionBounds(
        string DisplayName,
        double MinLatitude,
        double MaxLatitude,
        double MinLongitude,
        double MaxLongitude);
}
