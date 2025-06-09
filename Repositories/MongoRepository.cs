using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Bson;
using EventMngt.Data;

namespace EventMngt.Repositories;

public class MongoRepository<T> : Repository<T> where T : class
{
    public MongoRepository(MongoDbContext context, string collectionName) : base(context, collectionName)
    {
    }

    public override async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public override async Task<T?> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public override async Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter)
    {
        return await _collection.Find(filter).ToListAsync();
    }

    public override async Task<T?> FindOneAsync(FilterDefinition<T> filter)
    {
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public override async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public override async Task UpdateAsync(T entity)
    {
        var id = GetIdValue(entity);
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public override async Task DeleteAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }

    public override async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).AnyAsync();
    }

    public override async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.CountDocumentsAsync(predicate);
    }
} 