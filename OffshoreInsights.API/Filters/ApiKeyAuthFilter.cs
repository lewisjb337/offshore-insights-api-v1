using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OffshoreInsights.Application.Features.ApiKeys.Abstractions;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Filters;

/// <summary>
/// Action filter that validates the <c>X-Api-Key</c> request header against the database.
/// Applied via <see cref="Attributes.RequireApiKeyAttribute"/>.
/// Returns 401 if the header is absent and 403 if the key is invalid or expired.
/// </summary>
public class ApiKeyAuthFilter(IApiKeyValidator validator) : IAsyncActionFilter
{
    private const string ApiKeyHeader = "X-Api-Key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeader, out var values)
            || string.IsNullOrWhiteSpace(values))
        {
            context.Result = new UnauthorizedObjectResult(
                ApiResponse<object>.Fail($"API key is missing. Supply it in the '{ApiKeyHeader}' header."));
            return;
        }

        var isValid = await validator.ValidateAsync(values.ToString(), context.HttpContext.RequestAborted);

        if (!isValid)
        {
            context.Result = new ObjectResult(
                ApiResponse<object>.Fail("The provided API key is invalid or has expired."))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        await next();
    }
}
