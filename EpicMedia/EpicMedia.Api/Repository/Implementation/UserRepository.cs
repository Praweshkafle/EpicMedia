using EpicMedia.Api.Entity;
using EpicMedia.Api.Repository.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EpicMedia.Api.Repository.Implementation
{
    public class UserRepository:IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<User>("Users");
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<User> GetById(ObjectId id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task Create(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        public async Task<bool> Update(ObjectId id, User user)
        {
            var result = await _collection.ReplaceOneAsync(u => u.Id == id, user);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(ObjectId id)
        {
            var result = await _collection.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
