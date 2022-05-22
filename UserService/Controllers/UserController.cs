using DataInterface;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("")]
    [EnableCors("default")]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly RabbitMQService _rabbitMQService;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, RabbitMQService rabbitMQService, IConfiguration configuration)
        {
            _logger = logger;
            _userRepository = userRepository;
            _rabbitMQService = rabbitMQService;
            _configuration = configuration;
        }

        [HttpPut("username")]
        public async Task<IActionResult> RegisterAsync([FromBody] User user, [FromHeader]string token)
        {
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new string[] { _configuration["GOOGLE_CLIENT_ID"] }
            };

            GoogleJsonWebSignature.Payload? payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

            if (user.Name == null) throw new Exception("user.name is required");
            _logger.Log(LogLevel.Information, $"Updated user username to {user.Name}");

            return Ok(await _userRepository.RegisterOrUpdateUser(payload.Email, user.Name));
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me([FromHeader] string token)
        {
            var validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new string[] { _configuration["GOOGLE_CLIENT_ID"] }
            };

            GoogleJsonWebSignature.Payload? payload = await GoogleJsonWebSignature.ValidateAsync(token, validationSettings);

            if (token == null) throw new Exception("Call can't be done while the user is not logged in.");
            var user =  await _userRepository.Get(payload.Email);
            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string email)
        {
            _logger.LogInformation($"Deleted user {email}");
            await _userRepository.DeleteUser(email);
            return Ok();
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Test successfull");
        }
    }
}