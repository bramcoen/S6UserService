using DataInterface;
using Microsoft.Extensions.Configuration;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBRepository
{
    public class UserRepository : IUserRepository
    {

        MongoClient _dbClient;
        IMongoDatabase _mongoDatabase;
        IMongoCollection<User> _usersCollection;
        public UserRepository(IConfiguration configuration)
        {
            _dbClient = new MongoClient(configuration["MongoDBConnectionString"]);
            _mongoDatabase = _dbClient.GetDatabase("S6");
            _usersCollection = _mongoDatabase.GetCollection<User>("User_Users");
        }

        public async Task DeleteUser(string email)
        {
            await _usersCollection.DeleteOneAsync(i => i.Email == email);
        }

        /*  public async Task<User> SaveUserAsync(User user)
          {
              if (user.Id != null) throw new InvalidOperationException("Could not save user with an user ID");
              user.Id = Guid.NewGuid().ToString();
              if (user.Reference == null || user.Email == null) throw new InvalidDataException("Could not save an user without Reference or Email");
              await _usersCollection.InsertOneAsync(user);
              return user;
          }

          public async Task<Token> UpdateToken(string email, Token token)
          {
              var user = Get(email);
              if (user == null)
              {
                  throw new InvalidOperationException("Could not get token for non existing user");
              }
              var update = Builders<User>.Update.Set(i => i.Token, token.Value).Set(i => i.TokenValidity, token.ValidUntil);
              var result = await _usersCollection.UpdateOneAsync(i => i.Email == email, update);
              return token;

          }*/

        public async Task<User> Get(string email)
        {
            return await _usersCollection.Find(i => i.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> RegisterOrUpdateUser(string email, string username)
        {
            var user = await Get(email);
            if (user == null)
            {
                user = new User() { Email = email, Id = Guid.NewGuid().ToString() };
                await _usersCollection.InsertOneAsync(user);
            }

            var update = Builders<User>.Update.Set(i => i.Name, username);
            var result = await _usersCollection.UpdateOneAsync(i => i.Email == user.Email, update);
            user.Name = username;
            return user;
        }
    }
}