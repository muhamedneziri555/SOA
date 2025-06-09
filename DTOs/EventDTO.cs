namespace EventMngt.DTOs;

/// <summary>
/// Data Transfer Object for Event information
/// </summary>
public class EventDTO
{
    /// <summary>
    /// Unique identifier for the event
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Title of the event
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the event
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the event starts
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Date and time when the event ends
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Physical or virtual location of the event
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Maximum number of participants allowed
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Reference to the category this event belongs to
    /// </summary>
    public string CategoryId { get; set; } = string.Empty;

    /// <summary>
    /// Reference to the user who created the event
    /// </summary>
    public string OrganizerId { get; set; } = string.Empty;

    /// <summary>
    /// When the event was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Whether the event is currently active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Information about the event organizer
    /// </summary>
    public UserDto? Organizer { get; set; }

    /// <summary>
    /// Number of registrations for this event
    /// </summary>
    public long RegistrationCount { get; set; }
}

/// <summary>
/// Data Transfer Object for creating a new event
/// </summary>
public class CreateEventDTO
{
    /// <summary>
    /// Title of the event
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the event
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the event starts
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Date and time when the event ends
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Physical or virtual location of the event
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Maximum number of participants allowed
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Reference to the category this event belongs to
    /// </summary>
    public string CategoryId { get; set; } = string.Empty;

    /// <summary>
    /// Reference to the user who created the event
    /// </summary>
    public string OrganizerId { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for updating an existing event
/// </summary>
public class UpdateEventDTO
{
    /// <summary>
    /// New title for the event (optional)
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// New description for the event (optional)
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// New start date and time (optional)
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// New end date and time (optional)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// New location for the event (optional)
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// New capacity for the event (optional)
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// New category for the event (optional)
    /// </summary>
    public string? CategoryId { get; set; }

    /// <summary>
    /// New organizer for the event (optional)
    /// </summary>
    public string? OrganizerId { get; set; }

    /// <summary>
    /// New active status for the event (optional)
    /// </summary>
    public bool? IsActive { get; set; }
} 