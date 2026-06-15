using Microsoft.AspNetCore.Mvc;
using OffshoreInsights.API.Filters;
using OffshoreInsights.Domain.Enums;

namespace OffshoreInsights.API.Attributes;

/// <summary>
/// Restricts an action or controller to callers whose plan is at or above
/// <paramref name="minimumPlan"/>. Must be used alongside <see cref="RequireApiKeyAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireMinPlanAttribute : TypeFilterAttribute
{
    public RequireMinPlanAttribute(AccountPlan minimumPlan) : base(typeof(PlanAuthFilter))
    {
        Arguments = [minimumPlan];
    }
}
