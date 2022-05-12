namespace Models
{
    public class User
    {
        public User(string username)
        {
            Name = username;
        }
        public string Email { get; set; }

        public string Name { get; set; }
        public string Id { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenValidity { get; set; }
    }
}