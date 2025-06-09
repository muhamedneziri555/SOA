using MongoDB.Driver;
using EventMngt.Data;
using EventMngt.Models;

namespace EventMngt.Repositories;

public class FeedbackRepository : MongoRepository<Feedback>, IRepository<Feedback>
{
    public FeedbackRepository(MongoDbContext context) : base(context, "Feedback")
    {
    }
} 