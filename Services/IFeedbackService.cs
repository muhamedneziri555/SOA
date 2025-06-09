using EventMngt.Models;
using EventMngt.DTOs;

namespace EventMngt.Services;

public interface IFeedbackService
{
    Task<IEnumerable<FeedbackDTO>> GetAllFeedbackAsync();
    Task<FeedbackDTO?> GetFeedbackByIdAsync(string id);
    Task<IEnumerable<FeedbackDTO>> GetFeedbackByEventAsync(string eventId);
    Task<IEnumerable<FeedbackDTO>> GetFeedbackByUserAsync(string userId);
    Task<FeedbackDTO> CreateFeedbackAsync(CreateFeedbackDTO feedbackDto);
    Task<bool> UpdateFeedbackAsync(string id, UpdateFeedbackDTO feedbackDto);
    Task<bool> DeleteFeedbackAsync(string id);
} 