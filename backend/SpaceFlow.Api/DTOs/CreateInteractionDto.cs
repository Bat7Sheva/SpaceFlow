using System.ComponentModel.DataAnnotations;

namespace SpaceFlow.Api.DTOs;

public class CreateInteractionDto
{
    [Required]
    [MaxLength(50)]
    public string Channel { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Summary { get; set; } = string.Empty;

    public DateTime InteractionAt { get; set; }
    public DateTime? NextFollowUpAt { get; set; }
}