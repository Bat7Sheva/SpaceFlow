using Microsoft.EntityFrameworkCore;
using SpaceFlow.Api.Data;
using SpaceFlow.Api.DTOs;
using SpaceFlow.Api.Models;
using SpaceFlow.Api.Shared.Errors;

namespace SpaceFlow.Api.Services;

public class LeadService(AppDbContext dbContext) : ILeadService
{
    public async Task<IReadOnlyList<LeadClientDto>> GetLeadsAsync(string? search, string? status)
    {
        var query = dbContext.LeadsClients
            .AsNoTracking()
            .Include(x => x.Interactions)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim().ToLower();
            query = query.Where(x =>
                x.FullName.ToLower().Contains(searchTerm) ||
                x.Phone.ToLower().Contains(searchTerm) ||
                (x.Email != null && x.Email.ToLower().Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            var normalizedStatus = status.Trim().ToLower();
            query = query.Where(x => x.Status.ToLower() == normalizedStatus);
        }

        var items = await query
            .OrderByDescending(x => x.UpdatedAt)
            .ToListAsync();

        return items.Select(MapLead).ToList();
    }

    public async Task<IReadOnlyList<LeadClientDto>> GetTodayLeadsAsync()
    {
        var today = DateTime.UtcNow.Date;

        var items = await dbContext.LeadsClients
            .AsNoTracking()
            .Include(x => x.Interactions)
            .Where(x => x.NextFollowUpAt.HasValue
                        && x.NextFollowUpAt.Value.Date <= today
                        && x.Status.ToLower() != "closed")
            .OrderBy(x => x.NextFollowUpAt)
            .ToListAsync();

        return items.Select(MapLead).ToList();
    }

    public async Task<LeadClientDto> GetLeadByIdAsync(int id)
    {
        var lead = await dbContext.LeadsClients
            .AsNoTracking()
            .Include(x => x.Interactions)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (lead is null)
        {
            throw new NotFoundException($"Lead with id {id} was not found.");
        }

        return MapLead(lead);
    }

    public async Task<LeadClientDto> CreateLeadAsync(CreateLeadClientDto dto)
    {
        var now = DateTime.UtcNow;

        var lead = new LeadClient
        {
            FullName = dto.FullName.Trim(),
            Phone = dto.Phone.Trim(),
            Email = dto.Email?.Trim(),
            Source = dto.Source.Trim(),
            Status = dto.Status.Trim(),
            NextFollowUpAt = dto.NextFollowUpAt,
            Notes = dto.Notes?.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };

        dbContext.LeadsClients.Add(lead);
        await dbContext.SaveChangesAsync();

        return MapLead(lead);
    }

    public async Task<LeadClientDto> UpdateLeadAsync(int id, UpdateLeadClientDto dto)
    {
        var lead = await dbContext.LeadsClients
            .Include(x => x.Interactions)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (lead is null)
        {
            throw new NotFoundException($"Lead with id {id} was not found.");
        }

        lead.FullName = dto.FullName.Trim();
        lead.Phone = dto.Phone.Trim();
        lead.Email = dto.Email?.Trim();
        lead.Source = dto.Source.Trim();
        lead.Status = dto.Status.Trim();
        lead.NextFollowUpAt = dto.NextFollowUpAt;
        lead.Notes = dto.Notes?.Trim();
        lead.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return MapLead(lead);
    }

    public async Task DeleteLeadAsync(int id)
    {
        var lead = await dbContext.LeadsClients.FirstOrDefaultAsync(x => x.Id == id);
        if (lead is null)
        {
            throw new NotFoundException($"Lead with id {id} was not found.");
        }

        dbContext.LeadsClients.Remove(lead);
        await dbContext.SaveChangesAsync();
    }

    public async Task<InteractionDto> AddInteractionAsync(int leadClientId, CreateInteractionDto dto)
    {
        var lead = await dbContext.LeadsClients.FirstOrDefaultAsync(x => x.Id == leadClientId);
        if (lead is null)
        {
            throw new NotFoundException($"Lead with id {leadClientId} was not found.");
        }

        var interactionAt = dto.InteractionAt == default ? DateTime.UtcNow : dto.InteractionAt;

        var interaction = new Interaction
        {
            LeadClientId = leadClientId,
            Channel = dto.Channel.Trim(),
            Summary = dto.Summary.Trim(),
            InteractionAt = interactionAt,
            NextFollowUpAt = dto.NextFollowUpAt
        };

        dbContext.Interactions.Add(interaction);

        if (dto.NextFollowUpAt.HasValue)
        {
            lead.NextFollowUpAt = dto.NextFollowUpAt;
        }

        lead.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return MapInteraction(interaction);
    }

    private static LeadClientDto MapLead(LeadClient lead)
    {
        return new LeadClientDto
        {
            Id = lead.Id,
            FullName = lead.FullName,
            Phone = lead.Phone,
            Email = lead.Email,
            Source = lead.Source,
            Status = lead.Status,
            NextFollowUpAt = lead.NextFollowUpAt,
            Notes = lead.Notes,
            CreatedAt = lead.CreatedAt,
            UpdatedAt = lead.UpdatedAt,
            Interactions = lead.Interactions
                .OrderByDescending(x => x.InteractionAt)
                .Select(MapInteraction)
                .ToList()
        };
    }

    private static InteractionDto MapInteraction(Interaction interaction)
    {
        return new InteractionDto
        {
            Id = interaction.Id,
            LeadClientId = interaction.LeadClientId,
            Channel = interaction.Channel,
            Summary = interaction.Summary,
            InteractionAt = interaction.InteractionAt,
            NextFollowUpAt = interaction.NextFollowUpAt
        };
    }
}