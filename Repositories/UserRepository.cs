using MongoDB.Driver;
using EventMngt.Data;
using EventMngt.Models;

namespace EventMngt.Repositories;

public class UserRepository : MongoRepository<User>, IRepository<User>
{
    public UserRepository(MongoDbContext context) : base(context, "Users")
    {
    }
} 