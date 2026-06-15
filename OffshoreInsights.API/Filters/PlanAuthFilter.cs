using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OffshoreInsights.Domain.Enums;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Filters;

/// <summary>
/// Action filter that enforces a minimum plan requirement.
/// Applied via <see cref="Attributes.RequireMinPlanAttribute"/>.
/// Must run after <see cref="ApiKeyAuthFilter"/> which stores UserPlan in HttpContext.Items.
/// </summary>
public class PlanAuthFilter(AccountPlan minimumPlan) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var plan = context.HttpContext.Items.TryGetValue("UserPlan", out var p) && p is AccountPlan up
            ? up
            : AccountPlan.Free;

        if (plan < minimumPlan)
        {
            context.Result = new ObjectResult(
                ApiResponse<object>.Fail(
                    $"Your {plan} plan does not include this endpoint. " +
                    $"Upgrade to {minimumPlan} or above to access it."))
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        await next();
    }
}
