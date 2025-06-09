using EventMngt.Data;
using EventMngt.Models;
using EventMngt.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace EventMngt.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoSettings = configuration.GetSection("MongoDbSettings").Get<Models.MongoDbSettings>();
        if (mongoSettings == null)
            throw new InvalidOperationException("MongoDB settings are not configured");

        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);
        var context = new MongoDbContext(database);

        services.AddSingleton(context);
        services.AddScoped<IRepository<Event>>(sp => new MongoRepository<Event>(context, "events"));
        services.AddScoped<IRepository<Category>>(sp => new MongoRepository<Category>(context, "categories"));
        services.AddScoped<IRepository<Registration>>(sp => new MongoRepository<Registration>(context, "registrations"));
        services.AddScoped<IRepository<Feedback>>(sp => new MongoRepository<Feedback>(context, "feedbacks"));
        services.AddScoped<IRepository<Notification>>(sp => new MongoRepository<Notification>(context, "notifications"));
        services.AddScoped<IRepository<User>>(sp => new MongoRepository<User>(context, "users"));

        return services;
    }
} 