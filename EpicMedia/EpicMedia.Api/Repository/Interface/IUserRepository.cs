using EpicMedia.Api.Entity;
using MongoDB.Bson;

namespace EpicMedia.Api.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(ObjectId id);
        Task<User> GetByEmail(string username); 
        Task<User> GetByUserName(string email);
        Task Create(User user);
        Task<bool> Update(ObjectId id, User user);
        Task<bool> Delete(ObjectId id);
    }
}
