using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Api.Models;
using SpaceFlow.Api.Services;

namespace SpaceFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(ITestService testService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<string>>> GetStatusAsync()
    {
        var status = await testService.GetStatusAsync();

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Data = status,
            Message = "OK"
        });
    }
}
