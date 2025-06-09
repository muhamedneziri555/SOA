using MongoDB.Driver;
using EventMngt.Data;
using EventMngt.Models;

namespace EventMngt.Repositories;

public class RegistrationRepository : MongoRepository<Registration>, IRepository<Registration>
{
    public RegistrationRepository(MongoDbContext context) : base(context, "Registrations")
    {
    }
} 