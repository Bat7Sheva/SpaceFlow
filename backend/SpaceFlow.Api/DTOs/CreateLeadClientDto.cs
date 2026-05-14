using System.ComponentModel.DataAnnotations;

namespace SpaceFlow.Api.DTOs;

public class CreateLeadClientDto
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string Source { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    public DateTime? NextFollowUpAt { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }
}