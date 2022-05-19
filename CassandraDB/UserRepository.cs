using Cassandra;
using Cassandra.Mapping;
using DataInterface;
using Microsoft.Extensions.Configuration;
using Models;

namespace CassandraDB
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IConfiguration configuration)
        {
            cluster = Cluster.Builder()
              .AddContactPoints(configuration.GetSection("Cassandra")["ConnectionString"]).WithDefaultKeyspace("Data")
              .Build();

            // Connect to the nodes using a keyspace
            session = cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();

            mapper = new Mapper(session);
            mapper.Execute("Create table IF NOT EXISTS User(name text, email text, token text, tokenvalidity timestamp ,Primary key(name));");
        }
        Cluster cluster;
        ISession session;
        IMapper mapper;

        public Task DeleteUser(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User> SaveUserAsync(User user)
        {
            user.Id = Guid.NewGuid().ToString();
            var result = await mapper.InsertIfNotExistsAsync<User>(user);
            return result.Existing;
        }



        public Task<User> Get(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> RegisterUser(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUsername(string email, string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> RegisterOrUpdateUser(string email, string username)
        {
            throw new NotImplementedException();
        }
    }
}