using Domain;

namespace Application.Users
{
    public interface IUserData
    {
        Task CreateUser(User user);
        Task<User> GetUserWithDisplayName(string objectId);
    }
}
