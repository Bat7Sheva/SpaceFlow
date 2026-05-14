namespace SpaceFlow.Api.DTOs;

public class InteractionDto
{
    public int Id { get; set; }
    public int LeadClientId { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime InteractionAt { get; set; }
    public DateTime? NextFollowUpAt { get; set; }
}