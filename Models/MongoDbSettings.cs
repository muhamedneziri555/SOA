namespace EventMngt.Models;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;

    public static MongoDbSettings FromConfiguration(IConfiguration configuration)
    {
        return new MongoDbSettings
        {
            ConnectionString = configuration.GetSection("MongoDB:ConnectionString").Value ?? string.Empty,
            DatabaseName = configuration.GetSection("MongoDB:DatabaseName").Value ?? string.Empty
        };
    }
} 