using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OffshoreInsights.Application.Features.ApiKeys.Abstractions;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Filters;

/// <summary>
/// Action filter that validates the X-Api-Key request header, resolves the caller's plan,
/// and enforces the Free-tier monthly call limit.
/// Stores UserId and UserPlan in HttpContext.Items for downstream filters.
/// Returns 401 (missing key), 403 (invalid/expired), or 429 (rate limited).
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

        var result = await validator.ValidateAsync(values.ToString(), context.HttpContext.RequestAborted);

        if (!result.IsValid)
        {
            context.Result = new ObjectResult(
                ApiResponse<object>.Fail("The provided API key is invalid or has expired."))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        if (result.IsRateLimited)
        {
            context.Result = new ObjectResult(
                ApiResponse<object>.Fail("Monthly call limit reached. Upgrade your plan to continue."))
            {
                StatusCode = StatusCodes.Status429TooManyRequests
            };
            return;
        }

        // Make identity available to downstream filters (e.g. PlanAuthFilter) and handlers
        context.HttpContext.Items["UserId"]   = result.UserId;
        context.HttpContext.Items["UserPlan"] = result.Plan;

        await next();
    }
}
