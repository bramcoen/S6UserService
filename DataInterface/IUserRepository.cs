using Models;

namespace DataInterface
{
    public interface IUserRepository
    {
        //   public Task<User> SaveUserAsync(User user);
        /// <summary>
        /// Used to generate a new token and store this token into the database.
        /// </summary>
        public Task<User> Get(string email);
        //        public Task<Token> UpdateToken(string email, Token token);
        public Task<User> RegisterOrUpdateUser(string email, string username);
        public Task DeleteUser(string email);
    }
}