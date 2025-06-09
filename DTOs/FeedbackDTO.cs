using EventMngt.Models;

namespace EventMngt.DTOs;

public class FeedbackDTO
{
    public string Id { get; set; } = string.Empty;
    public string EventId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateFeedbackDTO
{
    public string EventId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
}

public class UpdateFeedbackDTO
{
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
} 