using EventMngt.Models;

namespace EventMngt.DTOs;

public class RegistrationDTO
{
    public string Id { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public RegistrationStatus Status { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Notes { get; set; }
}

public class CreateRegistrationDTO
{
    public string EventId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateRegistrationDTO
{
    public RegistrationStatus Status { get; set; }
    public string? Notes { get; set; }
} 