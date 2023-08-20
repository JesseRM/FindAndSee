using Domain;

namespace Persistence.Data
{
    public interface IUserData
    {
        Task<User> GetUser(string objectId);
    }
}