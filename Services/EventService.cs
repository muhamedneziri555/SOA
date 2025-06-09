using EventMngt.Models;
using EventMngt.DTOs;
using EventMngt.Repositories;
using EventMngt.Mappers;

namespace EventMngt.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventDTO>> GetAllEventsAsync()
    {
        var events = await _eventRepository.GetAllAsync();
        return events.Select(e => EventMapper.ToDTO(e));
    }

    public async Task<EventDTO?> GetEventByIdAsync(string id)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event == null) return null;

        var registrationCount = await _eventRepository.GetRegistrationCountAsync(id);
        return EventMapper.ToDTO(@event, registrationCount);
    }

    public async Task<IEnumerable<EventDTO>> GetUpcomingEventsAsync()
    {
        var events = await _eventRepository.GetUpcomingEventsAsync();
        return events.Select(e => EventMapper.ToDTO(e));
    }

    public async Task<IEnumerable<EventDTO>> GetEventsByOrganizerAsync(string organizerId)
    {
        var events = await _eventRepository.GetEventsByOrganizerAsync(organizerId);
        return events.Select(e => EventMapper.ToDTO(e));
    }

    public async Task<IEnumerable<EventDTO>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var events = await _eventRepository.GetEventsByDateRangeAsync(startDate, endDate);
        return events.Select(e => EventMapper.ToDTO(e));
    }

    public async Task<EventDTO> CreateEventAsync(CreateEventDTO eventDto)
    {
        var @event = EventMapper.ToModel(eventDto);
        await _eventRepository.AddAsync(@event);
        return EventMapper.ToDTO(@event);
    }

    public async Task<bool> UpdateEventAsync(string id, UpdateEventDTO eventDto)
    {
        var existingEvent = await _eventRepository.GetByIdAsync(id);
        if (existingEvent == null) return false;

        EventMapper.UpdateModel(existingEvent, eventDto);
        await _eventRepository.UpdateAsync(existingEvent);
        return true;
    }

    public async Task<bool> DeleteEventAsync(string id)
    {
        await _eventRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> IsEventFullAsync(string eventId)
    {
        return await _eventRepository.IsEventFullAsync(eventId);
    }

    public async Task<IEnumerable<EventDTO>> GetEventsByCategoryAsync(string categoryId)
    {
        var events = await _eventRepository.GetAllAsync();
        var filtered = events.Where(e => e.CategoryId == categoryId);
        return filtered.Select(e => EventMapper.ToDTO(e));
    }
} 