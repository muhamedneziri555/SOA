using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventMngt.DTOs;
using EventMngt.Models;
using EventMngt.Repositories;
using FluentValidation;
using EventMngt.Mappers;

namespace EventMngt.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IRepository<Feedback> _feedbackRepository;
    private readonly IRepository<Registration> _registrationRepository;
    private readonly IRepository<Event> _eventRepository;
    private readonly IValidator<FeedbackDTO> _validator;

    public FeedbackController(
        IRepository<Feedback> feedbackRepository,
        IRepository<Registration> registrationRepository,
        IRepository<Event> eventRepository,
        IValidator<FeedbackDTO> validator)
    {
        _feedbackRepository = feedbackRepository;
        _registrationRepository = registrationRepository;
        _eventRepository = eventRepository;
        _validator = validator;
    }

    [HttpGet("event/{eventId}")]
    public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetByEventId(string eventId)
    {
        var feedback = await _feedbackRepository.FindAsync(f => f.EventId == eventId);
        return Ok(feedback.Select(FeedbackMapper.ToDTO));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FeedbackDTO>> GetById(string id)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(id);
        if (feedback == null)
            return NotFound();

        return Ok(FeedbackMapper.ToDTO(feedback));
    }

    [HttpPost]
    public async Task<ActionResult<FeedbackDTO>> Create(CreateFeedbackDTO dto)
    {
        var ev = await _eventRepository.GetByIdAsync(dto.EventId);
        if (ev == null)
            return NotFound("Event not found");

        var feedback = FeedbackMapper.ToModel(dto);
        await _feedbackRepository.AddAsync(feedback);

        return CreatedAtAction(nameof(GetById), new { id = feedback.Id }, FeedbackMapper.ToDTO(feedback));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, UpdateFeedbackDTO dto)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(id);
        if (feedback == null)
            return NotFound();

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (feedback.UserId != userId)
            return Forbid();

        FeedbackMapper.UpdateModel(feedback, dto);
        await _feedbackRepository.UpdateAsync(feedback);

        return Ok(FeedbackMapper.ToDTO(feedback));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var feedback = await _feedbackRepository.GetByIdAsync(id);
        if (feedback == null)
            return NotFound();

        await _feedbackRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetByUserId(string userId)
    {
        var feedback = await _feedbackRepository.FindAsync(f => f.UserId == userId);
        return Ok(feedback.Select(FeedbackMapper.ToDTO));
    }

    [HttpGet("my-feedback")]
    public async Task<IActionResult> GetMyFeedback()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var feedback = await _feedbackRepository.FindAsync(f => f.UserId == userId);
        return Ok(feedback.Select(FeedbackMapper.ToDTO));
    }
} 