using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using EventMngt.Models;
using EventMngt.Repositories;
using MongoDB.Bson;

namespace EventMngt.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IMongoDatabase _database;
    private readonly IRepository<Event> _eventRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<User> _userRepository;

    public TestController(
        IMongoDatabase database,
        IRepository<Event> eventRepository,
        IRepository<Category> categoryRepository,
        IRepository<User> userRepository)
    {
        _database = database;
        _eventRepository = eventRepository;
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }

    [HttpGet("connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            // Test database connection
            var result = await _database.RunCommandAsync<dynamic>(new MongoDB.Bson.BsonDocument("ping", 1));
            
            // Get collection names
            var collections = await _database.ListCollectionNames().ToListAsync();
            
            // Test repository operations
            var eventCount = await _eventRepository.CountAsync(_ => true);
            var categoryCount = await _categoryRepository.CountAsync(_ => true);
            var userCount = await _userRepository.CountAsync(_ => true);

            return Ok(new
            {
                Status = "Connected",
                Database = _database.DatabaseNamespace.DatabaseName,
                Collections = collections,
                Counts = new
                {
                    Events = eventCount,
                    Categories = categoryCount,
                    Users = userCount
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = "Error",
                Message = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedData()
    {
        try
        {
            // Create a test category
            var category = new Category
            {
                Name = "Test Category",
                Description = "A test category for development",
                CreatedAt = DateTime.UtcNow
            };
            await _categoryRepository.AddAsync(category);

            // Create a test user
            var user = new User
            {
                Email = "test@example.com",
                UserName = "testuser",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "dummyhash", // In production, this should be properly hashed
                CreatedAt = DateTime.UtcNow,
                Roles = new List<ObjectId> { ObjectId.GenerateNewId() }
            };
            await _userRepository.AddAsync(user);

            // Create a test event
            var ev = new Event
            {
                Title = "Test Event",
                Description = "A test event for development",
                StartDate = DateTime.UtcNow.AddDays(7),
                EndDate = DateTime.UtcNow.AddDays(8),
                Location = "Test Location",
                Capacity = 100,
                CategoryId = category.Id,
                CreatedBy = user.Id.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            await _eventRepository.AddAsync(ev);

            return Ok(new
            {
                Status = "Success",
                Message = "Test data seeded successfully",
                Data = new
                {
                    CategoryId = category.Id,
                    UserId = user.Id,
                    EventId = ev.Id
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = "Error",
                Message = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }
} 