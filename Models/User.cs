using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public User()
        {

        }
        public User(string name,string email)
        {
            Name = name;
            Email = email;
        }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? Id { get; set; }
    }
}