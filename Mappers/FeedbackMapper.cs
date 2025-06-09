using EventMngt.Models;
using EventMngt.DTOs;

namespace EventMngt.Mappers;

public static class FeedbackMapper
{
    public static FeedbackDTO ToDTO(Feedback feedback)
    {
        return new FeedbackDTO
        {
            Id = feedback.Id,
            EventId = feedback.EventId,
            UserId = feedback.UserId,
            Rating = feedback.Rating,
            Comment = feedback.Comment,
            CreatedAt = feedback.CreatedAt
        };
    }

    public static Feedback ToModel(CreateFeedbackDTO dto)
    {
        return new Feedback
        {
            EventId = dto.EventId,
            UserId = dto.UserId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static void UpdateModel(Feedback feedback, UpdateFeedbackDTO dto)
    {
        feedback.Rating = dto.Rating;
        feedback.Comment = dto.Comment;
    }
} 