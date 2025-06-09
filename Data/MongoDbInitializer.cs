using MongoDB.Driver;
using MongoDB.Bson;
using EventMngt.Models;

namespace EventMngt.Data;

public static class MongoDbInitializer
{
    public static async Task InitializeAsync(IMongoDatabase database)
    {
        // Create collections if they don't exist
        var collections = new[] { "events", "categories", "registrations", "feedbacks", "notifications", "users" };
        foreach (var collectionName in collections)
        {
            try
            {
                if (!await CollectionExistsAsync(database, collectionName))
                {
                    await database.CreateCollectionAsync(collectionName);
                    Console.WriteLine($"Created collection: {collectionName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating collection {collectionName}: {ex.Message}");
            }
        }

        // Create indexes
        var eventsCollection = database.GetCollection<BsonDocument>("events");
        await eventsCollection.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("createdBy").Ascending("startDate").Ascending("categoryId")
            )
        });

        var registrationsCollection = database.GetCollection<BsonDocument>("registrations");
        await registrationsCollection.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("eventId").Ascending("userId")
            )
        });

        var feedbackCollection = database.GetCollection<BsonDocument>("feedbacks");
        await feedbackCollection.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("eventId").Ascending("userId")
            )
        });

        var notificationsCollection = database.GetCollection<BsonDocument>("notifications");
        await notificationsCollection.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("userId").Ascending("createdAt")
            )
        });

        var usersCollection = database.GetCollection<BsonDocument>("users");
        await usersCollection.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("email"),
                new CreateIndexOptions { Unique = true }
            )
        });

        // Seed sample data if collections are empty
        await SeedSampleDataAsync(database);
    }

    private static async Task SeedSampleDataAsync(IMongoDatabase database)
    {
        // Seed Categories
        var categoriesCollection = database.GetCollection<Category>("categories");
        if (await categoriesCollection.CountDocumentsAsync(new BsonDocument()) == 0)
        {
            var categories = new[]
            {
                new Category { Name = "Technology", Description = "Tech-related events" },
                new Category { Name = "Business", Description = "Business and entrepreneurship events" },
                new Category { Name = "Arts", Description = "Arts and culture events" }
            };
            await categoriesCollection.InsertManyAsync(categories);
            Console.WriteLine("Seeded categories");
        }

        // Seed Users
        var usersCollection = database.GetCollection<User>("users");
        if (await usersCollection.CountDocumentsAsync(new BsonDocument()) == 0)
        {
            var users = new[]
            {
                new User 
                { 
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Role = UserRole.Admin
                },
                new User 
                { 
                    UserName = "organizer@example.com",
                    Email = "organizer@example.com",
                    FirstName = "Event",
                    LastName = "Organizer",
                    Role = UserRole.Organizer
                }
            };
            await usersCollection.InsertManyAsync(users);
            Console.WriteLine("Seeded users");
        }

        // Seed Events
        var eventsCollection = database.GetCollection<Event>("events");
        if (await eventsCollection.CountDocumentsAsync(new BsonDocument()) == 0)
        {
            var techCategory = await categoriesCollection.Find(c => c.Name == "Technology").FirstOrDefaultAsync();
            var businessCategory = await categoriesCollection.Find(c => c.Name == "Business").FirstOrDefaultAsync();
            var adminUser = await usersCollection.Find(u => u.Email == "admin@example.com").FirstOrDefaultAsync();

            if (techCategory != null && businessCategory != null && adminUser != null)
            {
                var events = new[]
                {
                    new Event
                    {
                        Title = "Tech Conference 2024",
                        Description = "Annual technology conference",
                        StartDate = DateTime.UtcNow.AddDays(30),
                        EndDate = DateTime.UtcNow.AddDays(32),
                        Location = "Virtual",
                        Capacity = 1000,
                        CategoryId = techCategory.Id.ToString(),
                        CreatedBy = adminUser.Id.ToString(),
                        OrganizerId = adminUser.Id.ToString()
                    },
                    new Event
                    {
                        Title = "Business Workshop",
                        Description = "Business strategy workshop",
                        StartDate = DateTime.UtcNow.AddDays(15),
                        EndDate = DateTime.UtcNow.AddDays(15),
                        Location = "Main Office",
                        Capacity = 50,
                        CategoryId = businessCategory.Id.ToString(),
                        CreatedBy = adminUser.Id.ToString(),
                        OrganizerId = adminUser.Id.ToString()
                    }
                };
                await eventsCollection.InsertManyAsync(events);
                Console.WriteLine("Seeded events");
            }
        }
    }

    private static async Task<bool> CollectionExistsAsync(IMongoDatabase database, string collectionName)
    {
        var filter = new BsonDocument("name", collectionName);
        var collections = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
        return await collections.AnyAsync();
    }
} 