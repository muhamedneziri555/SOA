using EventMngt.Models;
using EventMngt.DTOs;

namespace EventMngt.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync();
    Task<NotificationDTO?> GetNotificationByIdAsync(string id);
    Task<IEnumerable<NotificationDTO>> GetNotificationsByUserAsync(string userId);
    Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO notificationDto);
    Task<bool> MarkNotificationAsReadAsync(string id);
    Task<bool> DeleteNotificationAsync(string id);
} 