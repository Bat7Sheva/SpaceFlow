using SpaceFlow.Api.DTOs;

namespace SpaceFlow.Api.Services;

public interface ILeadService
{
    Task<IReadOnlyList<LeadClientDto>> GetLeadsAsync(string? search, string? status);
    Task<IReadOnlyList<LeadClientDto>> GetTodayLeadsAsync();
    Task<LeadClientDto> GetLeadByIdAsync(int id);
    Task<LeadClientDto> CreateLeadAsync(CreateLeadClientDto dto);
    Task<LeadClientDto> UpdateLeadAsync(int id, UpdateLeadClientDto dto);
    Task DeleteLeadAsync(int id);
    Task<InteractionDto> AddInteractionAsync(int leadClientId, CreateInteractionDto dto);
}