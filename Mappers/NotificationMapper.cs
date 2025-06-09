using EventMngt.DTOs;
using EventMngt.Models;

namespace EventMngt.Mappers;

public static class NotificationMapper
{
    public static NotificationDTO ToDTO(Notification notification)
    {
        return new NotificationDTO
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt
        };
    }

    public static Notification ToModel(CreateNotificationDTO dto)
    {
        return new Notification
        {
            Title = dto.Title,
            Message = dto.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static void UpdateModel(Notification notification, UpdateNotificationDTO dto)
    {
        notification.IsRead = dto.IsRead;
        notification.UpdatedAt = DateTime.UtcNow;
    }
} 