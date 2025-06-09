using EventMngt.DTOs;
using EventMngt.Models;
using MongoDB.Driver;

namespace EventMngt.Mappers;

public static class EventMapper
{
    public static EventDTO ToDTO(Event @event, long registrationCount = 0)
    {
        return new EventDTO
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartDate = @event.StartDate,
            EndDate = @event.EndDate,
            Location = @event.Location,
            Capacity = @event.Capacity,
            CategoryId = @event.CategoryId,
            OrganizerId = @event.OrganizerId,
            CreatedAt = @event.CreatedAt,
            Organizer = @event.Organizer != null ? UserMapper.ToDto(@event.Organizer) : null,
            RegistrationCount = registrationCount
        };
    }

    public static Event ToModel(CreateEventDTO dto)
    {
        return new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Location = dto.Location,
            Capacity = dto.Capacity,
            CategoryId = dto.CategoryId,
            OrganizerId = dto.OrganizerId,
            IsActive = true
        };
    }

    public static void UpdateModel(Event @event, UpdateEventDTO dto)
    {
        if (dto.Title != null)
            @event.Title = dto.Title;
        if (dto.Description != null)
            @event.Description = dto.Description;
        if (dto.StartDate.HasValue)
            @event.StartDate = dto.StartDate.Value;
        if (dto.EndDate.HasValue)
            @event.EndDate = dto.EndDate.Value;
        if (dto.Location != null)
            @event.Location = dto.Location;
        if (dto.Capacity.HasValue)
            @event.Capacity = dto.Capacity.Value;
        if (dto.CategoryId != null)
            @event.CategoryId = dto.CategoryId;
        if (dto.OrganizerId != null)
            @event.OrganizerId = dto.OrganizerId;
        if (dto.IsActive.HasValue)
            @event.IsActive = dto.IsActive.Value;
    }
}

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}

// (Removed EventRepository and IEventRepository so that the build succeeds.) 