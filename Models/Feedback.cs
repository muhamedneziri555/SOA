using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventMngt.Models;

public class Feedback
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    [BsonElement("eventId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string EventId { get; set; } = string.Empty;
    
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;
    
    [BsonElement("rating")]
    public int Rating { get; set; }
    
    [BsonElement("comment")]
    public string Comment { get; set; } = string.Empty;
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    [BsonIgnore]
    public Event? Event { get; set; }
    
    [BsonIgnore]
    public User? User { get; set; }
} 