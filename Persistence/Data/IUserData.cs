using Domain;

namespace Persistence.Data
{
    public interface IUserData
    {
        Task<User> GetUserWithDisplayName(string objectId);
    }
}
