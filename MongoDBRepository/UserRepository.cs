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
            _dbClient = new MongoClient(configuration.GetSection("MongoDB")["ConnectionString"]);
            _mongoDatabase = _dbClient.GetDatabase("S6");
            _usersCollection = _mongoDatabase.GetCollection<User>("Users");
        }
        public async Task DeleteUser(string username)
        {
            await _usersCollection.DeleteOneAsync(i => i.Name == username);
        }

        public async Task<User> SaveUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
            return user;
        }

        public async Task<Token> UpdateToken(string username, Token token)
        {
            var update = Builders<User>.Update.Set(i => i.Token, token.Value).Set(i => i.TokenValidity, token.ValidUntil);
            var result = await _usersCollection.UpdateOneAsync(i => i.Name == username, update);
            return token;
        }
    }
}