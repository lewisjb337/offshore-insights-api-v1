using MediatR;
using Microsoft.AspNetCore.Mvc;
using OffshoreInsights.API.Attributes;
using OffshoreInsights.Application.Features.Account.Queries;
using OffshoreInsights.Application.Features.Account.Responses;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Controllers;

[ApiController]
[RequireApiKey]
public class AccountController(ISender sender) : BaseController
{
    /// <summary>
    /// Retrieves the current plan, API call usage for this billing period, and rate limit status
    /// for the authenticated account.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>Account plan, usage, and rate limit details.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAccountAsync(CancellationToken cancellationToken)
    {
        try
        {
            var userId   = HttpContext.Items["UserId"] as string ?? string.Empty;
            var response = await sender.Send(new GetAccountQuery(userId), cancellationToken);

            return Ok(ApiResponse<AccountResponse>.Ok(response));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<AccountResponse>.Fail(ex.Message));
        }
    }
}
