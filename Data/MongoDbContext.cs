using MongoDB.Driver;
using EventMngt.Models;

namespace EventMngt.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoDatabase database)
    {
        _database = database;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }

    public IMongoCollection<Event> Events => _database.GetCollection<Event>("events");
    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("categories");
    public IMongoCollection<Registration> Registrations => _database.GetCollection<Registration>("registrations");
    public IMongoCollection<Feedback> Feedbacks => _database.GetCollection<Feedback>("feedbacks");
    public IMongoCollection<Notification> Notifications => _database.GetCollection<Notification>("notifications");
} 