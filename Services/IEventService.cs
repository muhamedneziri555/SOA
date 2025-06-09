using EventMngt.Models;
using EventMngt.DTOs;

namespace EventMngt.Services;

public interface IEventService
{
    Task<IEnumerable<EventDTO>> GetAllEventsAsync();
    Task<EventDTO?> GetEventByIdAsync(string id);
    Task<IEnumerable<EventDTO>> GetUpcomingEventsAsync();
    Task<IEnumerable<EventDTO>> GetEventsByOrganizerAsync(string organizerId);
    Task<IEnumerable<EventDTO>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<EventDTO>> GetEventsByCategoryAsync(string categoryId);
    Task<EventDTO> CreateEventAsync(CreateEventDTO eventDto);
    Task<bool> UpdateEventAsync(string id, UpdateEventDTO eventDto);
    Task<bool> DeleteEventAsync(string id);
    Task<bool> IsEventFullAsync(string eventId);
} 