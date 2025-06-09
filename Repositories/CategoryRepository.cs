using MongoDB.Driver;
using EventMngt.Data;
using EventMngt.Models;

namespace EventMngt.Repositories;

public class CategoryRepository : MongoRepository<Category>, IRepository<Category>
{
    public CategoryRepository(MongoDbContext context) : base(context, "Categories")
    {
    }
} 