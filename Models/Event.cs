using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventMngt.Models;

public class Event
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("startDate")]
    public DateTime StartDate { get; set; }

    [BsonElement("endDate")]
    public DateTime EndDate { get; set; }

    [BsonElement("location")]
    public string Location { get; set; } = string.Empty;

    [BsonElement("capacity")]
    public int Capacity { get; set; }

    [BsonElement("categoryId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = string.Empty;

    [BsonElement("createdBy")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string CreatedBy { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("organizerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OrganizerId { get; set; } = string.Empty;

    [BsonElement("organizer")]
    public User? Organizer { get; set; }

    // Navigation properties
    [BsonIgnore]
    public Category? Category { get; set; }
} 