using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventMngt.DTOs;
using EventMngt.Models;
using EventMngt.Repositories;
using EventMngt.Validators;
using EventMngt.Mappers;
using FluentValidation;

namespace EventMngt.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IValidator<CreateNotificationDTO> _createValidator;
    private readonly IValidator<UpdateNotificationDTO> _updateValidator;

    public NotificationController(
        IRepository<Notification> notificationRepository,
        IValidator<CreateNotificationDTO> createValidator,
        IValidator<UpdateNotificationDTO> updateValidator)
    {
        _notificationRepository = notificationRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotifications()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var notifications = await _notificationRepository.FindAsync(n => n.UserId == userId);
        return Ok(notifications.Select(NotificationMapper.ToDTO));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null)
        {
            return NotFound();
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || notification.UserId != userId)
        {
            return Forbid();
        }

        return Ok(NotificationMapper.ToDTO(notification));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNotificationDTO dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var notification = NotificationMapper.ToModel(dto);
        notification.UserId = userId;
        await _notificationRepository.AddAsync(notification);

        return CreatedAtAction(nameof(GetById), new { id = notification.Id }, NotificationMapper.ToDTO(notification));
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null)
        {
            return NotFound();
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || notification.UserId != userId)
        {
            return Forbid();
        }

        var updateDto = new UpdateNotificationDTO { IsRead = true };
        NotificationMapper.UpdateModel(notification, updateDto);
        await _notificationRepository.UpdateAsync(notification);

        return Ok(NotificationMapper.ToDTO(notification));
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var notifications = await _notificationRepository.FindAsync(n => 
            n.UserId == userId && !n.IsRead);

        foreach (var notification in notifications)
        {
            var updateDto = new UpdateNotificationDTO { IsRead = true };
            NotificationMapper.UpdateModel(notification, updateDto);
            await _notificationRepository.UpdateAsync(notification);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var notification = await _notificationRepository.GetByIdAsync(id);
        if (notification == null)
        {
            return NotFound();
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId) || notification.UserId != userId)
        {
            return Forbid();
        }

        await _notificationRepository.DeleteAsync(id);
        return NoContent();
    }
} 