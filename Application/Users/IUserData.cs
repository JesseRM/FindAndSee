using Domain;

namespace Application.Users
{
    public interface IUserData
    {
        Task CreateUser(UserCreate user);
        Task<User> GetUserWithDisplayName(string objectId);
    }
}
