using MongoDB.Driver;
using EventMngt.Models;
using EventMngt.DTOs;
using EventMngt.Data;

namespace EventMngt.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IMongoCollection<Feedback> _feedbacks;

    public FeedbackService(MongoDbContext context)
    {
        _feedbacks = context.Feedbacks;
    }

    public async Task<IEnumerable<FeedbackDTO>> GetAllFeedbackAsync()
    {
        var feedbacks = await _feedbacks.Find(_ => true).ToListAsync();
        return feedbacks.Select(f => new FeedbackDTO
        {
            Id = f.Id,
            EventId = f.EventId,
            UserId = f.UserId,
            Rating = f.Rating,
            Comment = f.Comment,
            CreatedAt = f.CreatedAt
        });
    }

    public async Task<FeedbackDTO?> GetFeedbackByIdAsync(string id)
    {
        var feedback = await _feedbacks.Find(f => f.Id == id).FirstOrDefaultAsync();
        if (feedback == null) return null;

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

    public async Task<IEnumerable<FeedbackDTO>> GetFeedbackByEventAsync(string eventId)
    {
        var feedbacks = await _feedbacks.Find(f => f.EventId == eventId).ToListAsync();
        return feedbacks.Select(f => new FeedbackDTO
        {
            Id = f.Id,
            EventId = f.EventId,
            UserId = f.UserId,
            Rating = f.Rating,
            Comment = f.Comment,
            CreatedAt = f.CreatedAt
        });
    }

    public async Task<IEnumerable<FeedbackDTO>> GetFeedbackByUserAsync(string userId)
    {
        var feedbacks = await _feedbacks.Find(f => f.UserId == userId).ToListAsync();
        return feedbacks.Select(f => new FeedbackDTO
        {
            Id = f.Id,
            EventId = f.EventId,
            UserId = f.UserId,
            Rating = f.Rating,
            Comment = f.Comment,
            CreatedAt = f.CreatedAt
        });
    }

    public async Task<FeedbackDTO> CreateFeedbackAsync(CreateFeedbackDTO feedbackDto)
    {
        var feedback = new Feedback
        {
            EventId = feedbackDto.EventId,
            UserId = feedbackDto.UserId,
            Rating = feedbackDto.Rating,
            Comment = feedbackDto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _feedbacks.InsertOneAsync(feedback);
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

    public async Task<bool> UpdateFeedbackAsync(string id, UpdateFeedbackDTO feedbackDto)
    {
        var update = Builders<Feedback>.Update
            .Set(f => f.Rating, feedbackDto.Rating)
            .Set(f => f.Comment, feedbackDto.Comment);

        var result = await _feedbacks.UpdateOneAsync(f => f.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteFeedbackAsync(string id)
    {
        var result = await _feedbacks.DeleteOneAsync(f => f.Id == id);
        return result.DeletedCount > 0;
    }
} 