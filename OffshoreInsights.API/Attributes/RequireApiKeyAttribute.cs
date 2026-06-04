using Microsoft.AspNetCore.Mvc;
using OffshoreInsights.API.Filters;

namespace OffshoreInsights.API.Attributes;

/// <summary>
/// Enforces API key authentication on the decorated controller or action.
/// The key must be supplied in the <c>X-Api-Key</c> request header.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RequireApiKeyAttribute : ServiceFilterAttribute
{
    public RequireApiKeyAttribute() : base(typeof(ApiKeyAuthFilter)) { }
}
