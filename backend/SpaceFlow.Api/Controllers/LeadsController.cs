using Microsoft.AspNetCore.Mvc;
using SpaceFlow.Api.DTOs;
using SpaceFlow.Api.Models;
using SpaceFlow.Api.Services;

namespace SpaceFlow.Api.Controllers;

[ApiController]
[Route("api/leads")]
public class LeadsController(ILeadService leadService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LeadClientDto>>>> GetLeadsAsync(
        [FromQuery] string? search,
        [FromQuery] string? status)
    {
        var leads = await leadService.GetLeadsAsync(search, status);

        return Ok(new ApiResponse<IReadOnlyList<LeadClientDto>>
        {
            Success = true,
            Data = leads,
            Message = "Leads retrieved successfully"
        });
    }

    [HttpGet("today")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<LeadClientDto>>>> GetTodayLeadsAsync()
    {
        var leads = await leadService.GetTodayLeadsAsync();

        return Ok(new ApiResponse<IReadOnlyList<LeadClientDto>>
        {
            Success = true,
            Data = leads,
            Message = "Today's follow-up leads retrieved successfully"
        });
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<LeadClientDto>>> GetLeadByIdAsync(int id)
    {
        var lead = await leadService.GetLeadByIdAsync(id);

        return Ok(new ApiResponse<LeadClientDto>
        {
            Success = true,
            Data = lead,
            Message = "Lead retrieved successfully"
        });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LeadClientDto>>> CreateLeadAsync([FromBody] CreateLeadClientDto dto)
    {
        var createdLead = await leadService.CreateLeadAsync(dto);

        return CreatedAtAction(nameof(GetLeadByIdAsync), new { id = createdLead.Id }, new ApiResponse<LeadClientDto>
        {
            Success = true,
            Data = createdLead,
            Message = "Lead created successfully"
        });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<LeadClientDto>>> UpdateLeadAsync(int id, [FromBody] UpdateLeadClientDto dto)
    {
        var updatedLead = await leadService.UpdateLeadAsync(id, dto);

        return Ok(new ApiResponse<LeadClientDto>
        {
            Success = true,
            Data = updatedLead,
            Message = "Lead updated successfully"
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteLeadAsync(int id)
    {
        await leadService.DeleteLeadAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Data = null,
            Message = "Lead deleted successfully"
        });
    }

    [HttpPost("{id:int}/interactions")]
    public async Task<ActionResult<ApiResponse<InteractionDto>>> AddInteractionAsync(int id, [FromBody] CreateInteractionDto dto)
    {
        var interaction = await leadService.AddInteractionAsync(id, dto);

        return Ok(new ApiResponse<InteractionDto>
        {
            Success = true,
            Data = interaction,
            Message = "Interaction added successfully"
        });
    }
}