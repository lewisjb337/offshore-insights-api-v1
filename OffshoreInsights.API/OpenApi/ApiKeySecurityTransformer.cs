using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace OffshoreInsights.API.OpenApi;

/// <summary>
/// Registers the X-Api-Key security scheme in the OpenAPI document and applies
/// it as a global security requirement so Scalar shows the auth field on every endpoint.
/// </summary>
public class ApiKeySecurityTransformer : IOpenApiDocumentTransformer
{
    private const string SchemeName = "ApiKey";

    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();

        document.Components.SecuritySchemes[SchemeName] = new OpenApiSecurityScheme
        {
            Type        = SecuritySchemeType.ApiKey,
            In          = ParameterLocation.Header,
            Name        = "X-Api-Key",
            Description = "API key — obtain one from the Developer Portal at /developer.",
        };

        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            { new OpenApiSecuritySchemeReference(SchemeName), [] }
        });

        return Task.CompletedTask;
    }
}
