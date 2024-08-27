using Domain;

namespace Application.Users
{
    public interface IUserData
    {
        Task<int> CreateUser(User user);
        Task<User> GetUserWithDisplayName(string objectId);
    }
}
