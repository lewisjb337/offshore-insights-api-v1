using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace OffshoreInsights.API.OpenApi;

/// <summary>
/// Registers the X-Api-Key security scheme in the OpenAPI document and applies
/// it as a global security requirement so Scalar shows the auth field on every
/// endpoint and can pre-populate it with a default value.
/// </summary>
public class ApiKeySecurityTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        // ── 1. Register the scheme under components ───────────────
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

        document.Components.SecuritySchemes["ApiKey"] = new OpenApiSecurityScheme
        {
            Type        = SecuritySchemeType.ApiKey,
            In          = ParameterLocation.Header,
            Name        = "X-Api-Key",
            Description = "API key — obtain one from the Developer Portal at /developer.",
        };

        // ── 2. Apply globally so every operation inherits it ──────
        var requirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id   = "ApiKey",
                    }
                },
                Array.Empty<string>()
            }
        };

        document.SecurityRequirements ??= new List<OpenApiSecurityRequirement>();
        document.SecurityRequirements.Add(requirement);

        return Task.CompletedTask;
    }
}
