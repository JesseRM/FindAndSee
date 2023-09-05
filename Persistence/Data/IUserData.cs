using Domain;

namespace Persistence.Data
{
    public interface IUserData
    {
        Task CreateUser(UserCreate user);
        Task<User> GetUserWithDisplayName(string objectId);
    }
}
