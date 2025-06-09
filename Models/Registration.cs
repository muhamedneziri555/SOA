using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventMngt.Models;

public class Registration
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("eventId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string EventId { get; set; } = string.Empty;
    
    [BsonElement("userId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;
    
    [BsonElement("status")]
    public RegistrationStatus Status { get; set; }
    
    [BsonElement("registrationDate")]
    public DateTime RegistrationDate { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
    
    [BsonElement("notes")]
    public string? Notes { get; set; }

    // Navigation properties
    [BsonIgnore]
    public Event? Event { get; set; }
    
    [BsonIgnore]
    public User? User { get; set; }
} 