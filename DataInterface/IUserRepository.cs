using Models;

namespace DataInterface
{
    public interface IUserRepository
    {
        public Task<User> SaveUserAsync(User user);
        /// <summary>
        /// Used to generate a new token and store this token into the database.
        /// </summary>
        public Task<Token> UpdateToken(string username, Token token);
        public Task DeleteUser(string username);
    }
}