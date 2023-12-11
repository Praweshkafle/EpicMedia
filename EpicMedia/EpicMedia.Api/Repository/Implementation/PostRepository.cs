using EpicMedia.Api.Entity;
using EpicMedia.Api.Repository.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EpicMedia.Api.Repository.Implementation
{
    public class PostRepository : IPostRepository
    {
        private readonly MongoClient Client;
        private readonly IMongoDatabase DB;
        private readonly IMongoCollection<Posts> _collection;

        public PostRepository(IConfiguration configuration)
        {
            Client = new MongoClient(configuration["MyKey"]);
            DB = Client.GetDatabase("EpicMedia");
            _collection = DB.GetCollection<Posts>("Post");
        }
        public async Task<IEnumerable<Posts>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Posts> GetById(ObjectId id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }


        public async Task Create(Posts post)
        {
            await _collection.InsertOneAsync(post);
        }

        public async Task<bool> Update(ObjectId id, Posts post)
        {
            var result = await _collection.ReplaceOneAsync(u => u.Id == id, post);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> Delete(ObjectId id)
        {
            var result = await _collection.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }

    }
}
