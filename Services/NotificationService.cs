using MongoDB.Driver;
using EventMngt.Models;
using EventMngt.DTOs;
using EventMngt.Data;

namespace EventMngt.Services;

public class NotificationService : INotificationService
{
    private readonly IMongoCollection<Notification> _notifications;

    public NotificationService(MongoDbContext context)
    {
        _notifications = context.Notifications;
    }

    public async Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync()
    {
        var notifications = await _notifications.Find(_ => true).ToListAsync();
        return notifications.Select(n => new NotificationDTO
        {
            Id = n.Id,
            UserId = n.UserId,
            Title = n.Title,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        });
    }

    public async Task<NotificationDTO?> GetNotificationByIdAsync(string id)
    {
        var notification = await _notifications.Find(n => n.Id == id).FirstOrDefaultAsync();
        if (notification == null) return null;

        return new NotificationDTO
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }

    public async Task<IEnumerable<NotificationDTO>> GetNotificationsByUserAsync(string userId)
    {
        var notifications = await _notifications.Find(n => n.UserId == userId)
            .Sort(Builders<Notification>.Sort.Descending(n => n.CreatedAt))
            .ToListAsync();
        return notifications.Select(n => new NotificationDTO
        {
            Id = n.Id,
            UserId = n.UserId,
            Title = n.Title,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        });
    }

    public async Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO notificationDto)
    {
        var notification = new Notification
        {
            UserId = notificationDto.UserId,
            Title = notificationDto.Title,
            Message = notificationDto.Message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notifications.InsertOneAsync(notification);
        return new NotificationDTO
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
    }

    public async Task<bool> MarkNotificationAsReadAsync(string id)
    {
        var update = Builders<Notification>.Update
            .Set(n => n.IsRead, true);

        var result = await _notifications.UpdateOneAsync(n => n.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteNotificationAsync(string id)
    {
        var result = await _notifications.DeleteOneAsync(n => n.Id == id);
        return result.DeletedCount > 0;
    }
} 