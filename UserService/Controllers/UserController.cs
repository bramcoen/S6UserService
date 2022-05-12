using DataInterface;
using Microsoft.AspNetCore.Mvc;
using Models;
using RabbitMQ.Client;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult Register(string email, string name)
        {
            _logger.Log(LogLevel.Information, $"Registered user with email: {email} and username {name}");
            return Ok(_userRepository.SaveUserAsync(new User(name) { Email = email }));
        }

        [HttpGet("token")]
        public async Task<Token> RefreshToken(string username)
        {
            _logger.LogInformation($"Refreshed token for user {username}");
            return await _userRepository.UpdateToken(username, new Token(Guid.NewGuid().ToString(), DateTime.Now.AddMinutes(240)));
        }

        [HttpGet("token")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            _logger.LogInformation($"Deleted user {username}");
            await _userRepository.DeleteUser(username);
            return Ok();
        }
    }
}