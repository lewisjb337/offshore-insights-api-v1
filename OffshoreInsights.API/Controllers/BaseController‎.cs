using Microsoft.AspNetCore.Mvc;
using OffshoreInsights.Domain.Shared;

namespace OffshoreInsights.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected IActionResult OkResponse<T>(T payload) =>
        Ok(ApiResponse<T>.Ok(payload));

    protected IActionResult FailResponse(string error, int? errorCode = null) => 
        BadRequest(ApiResponse<object>.Fail(error, errorCode));
}