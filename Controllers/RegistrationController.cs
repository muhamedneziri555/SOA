using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventMngt.DTOs;
using EventMngt.Models;
using EventMngt.Repositories;
using EventMngt.Validators;
using EventMngt.Mappers;
using FluentValidation;
using MongoDB.Driver;

namespace EventMngt.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RegistrationController : ControllerBase
{
    private readonly IRepository<Registration> _registrationRepository;
    private readonly IRepository<Event> _eventRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IValidator<CreateRegistrationDTO> _createValidator;
    private readonly IValidator<UpdateRegistrationDTO> _updateValidator;

    public RegistrationController(
        IRepository<Registration> registrationRepository,
        IRepository<Event> eventRepository,
        IRepository<User> userRepository,
        IValidator<CreateRegistrationDTO> createValidator,
        IValidator<UpdateRegistrationDTO> updateValidator)
    {
        _registrationRepository = registrationRepository;
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegistrationDTO>>> GetAll()
    {
        var registrations = await _registrationRepository.GetAllAsync();
        return Ok(registrations.Select(RegistrationMapper.ToDTO));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RegistrationDTO>> GetById(string id)
    {
        var registration = await _registrationRepository.GetByIdAsync(id);
        if (registration == null)
            return NotFound();

        return Ok(RegistrationMapper.ToDTO(registration));
    }

    [HttpGet("event/{eventId}")]
    public async Task<ActionResult<IEnumerable<RegistrationDTO>>> GetByEventId(string eventId)
    {
        var filter = Builders<Registration>.Filter.Eq(r => r.EventId, eventId);
        var registrations = await _registrationRepository.FindAsync(filter);
        return Ok(registrations.Select(RegistrationMapper.ToDTO));
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<RegistrationDTO>>> GetByUserId(string userId)
    {
        var filter = Builders<Registration>.Filter.Eq(r => r.UserId, userId);
        var registrations = await _registrationRepository.FindAsync(filter);
        return Ok(registrations.Select(RegistrationMapper.ToDTO));
    }

    [HttpPost]
    public async Task<ActionResult<RegistrationDTO>> Create(CreateRegistrationDTO dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var @event = await _eventRepository.GetByIdAsync(dto.EventId);
        if (@event == null)
            return BadRequest("Event not found");

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return BadRequest("User not found");

        var filter = Builders<Registration>.Filter.And(
            Builders<Registration>.Filter.Eq(r => r.EventId, dto.EventId),
            Builders<Registration>.Filter.Eq(r => r.UserId, userId)
        );
        var existingRegistration = await _registrationRepository.FindOneAsync(filter);
        if (existingRegistration != null)
            return BadRequest("User is already registered for this event");

        var registration = RegistrationMapper.ToModel(dto);
        registration.UserId = userId; // Set from current user
        await _registrationRepository.AddAsync(registration);

        return CreatedAtAction(nameof(GetById), new { id = registration.Id }, RegistrationMapper.ToDTO(registration));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateRegistrationDTO dto)
    {
        var registration = await _registrationRepository.GetByIdAsync(id);
        if (registration == null)
            return NotFound();

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || registration.UserId != userId)
            return Forbid();

        RegistrationMapper.UpdateModel(registration, dto);
        await _registrationRepository.UpdateAsync(registration);

        return Ok(RegistrationMapper.ToDTO(registration));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var registration = await _registrationRepository.GetByIdAsync(id);
        if (registration == null)
            return NotFound();

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || registration.UserId != userId)
            return Forbid();

        await _registrationRepository.DeleteAsync(id);
        return NoContent();
    }
} 