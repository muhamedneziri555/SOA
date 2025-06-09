using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventMngt.Services;
using EventMngt.DTOs;
using EventMngt.Models;
using EventMngt.Repositories;
using EventMngt.Mappers;
using FluentValidation;

namespace EventMngt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IRepository<Event> _eventRepository;
    private readonly IRepository<Registration> _registrationRepository;
    private readonly IValidator<CreateEventDTO> _createValidator;
    private readonly IValidator<UpdateEventDTO> _updateValidator;

    public EventController(
        IEventService eventService,
        IRepository<Event> eventRepository,
        IRepository<Registration> registrationRepository,
        IValidator<CreateEventDTO> createValidator,
        IValidator<UpdateEventDTO> updateValidator)
    {
        _eventService = eventService;
        _eventRepository = eventRepository;
        _registrationRepository = registrationRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDTO>> GetEventById(string id)
    {
        var eventItem = await _eventService.GetEventByIdAsync(id);
        if (eventItem == null)
        {
            return NotFound();
        }
        return Ok(eventItem);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<EventDTO>>> GetEventsByCategory(string categoryId)
    {
        var events = await _eventService.GetEventsByCategoryAsync(categoryId);
        return Ok(events);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<EventDTO>> CreateEvent(CreateEventDTO eventDto)
    {
        try
        {
            // Get the current user's ID from the claims
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            eventDto.OrganizerId = userId;
            var eventItem = await _eventService.CreateEventAsync(eventDto);
            return CreatedAtAction(nameof(GetEventById), new { id = eventItem.Id }, eventItem);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(string id, UpdateEventDTO eventDto)
    {
        // Get the current user's ID from the claims
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // Check if the user is the organizer
        var existingEvent = await _eventService.GetEventByIdAsync(id);
        if (existingEvent == null)
        {
            return NotFound();
        }

        if (existingEvent.OrganizerId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var success = await _eventService.UpdateEventAsync(id, eventDto);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(string id)
    {
        // Get the current user's ID from the claims
        var userId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // Check if the user is the organizer
        var existingEvent = await _eventService.GetEventByIdAsync(id);
        if (existingEvent == null)
        {
            return NotFound();
        }

        if (existingEvent.OrganizerId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        var success = await _eventService.DeleteEventAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Search query is required");
        }

        var events = await _eventRepository.FindAsync(e =>
            e.Title.Contains(query) ||
            e.Description.Contains(query));

        var eventDtos = new List<EventDTO>();
        foreach (var ev in events)
        {
            var registrationCount = await _registrationRepository.CountAsync(r =>
                r.EventId == ev.Id &&
                r.Status == RegistrationStatus.Approved);

            eventDtos.Add(EventMapper.ToDTO(ev, registrationCount));
        }

        return Ok(eventDtos);
    }
} 