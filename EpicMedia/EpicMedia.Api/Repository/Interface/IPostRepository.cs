using EpicMedia.Api.Entity;
using MongoDB.Bson;

namespace EpicMedia.Api.Repository.Interface
{
    public interface IPostRepository
    {
        Task<IEnumerable<Posts>> GetAll();
        Task<Posts> GetById(ObjectId id);
        Task Create(Posts user);
        Task<bool> Update(ObjectId id, Posts post);
        Task<bool> Delete(ObjectId id);
    }
}
