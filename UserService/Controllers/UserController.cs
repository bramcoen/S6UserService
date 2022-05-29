using DataInterface;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using UserService.Services;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace UserService.Controllers
{
    [ApiController]
    [Route("")]
    [EnableCors("default")]
    public class UserController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly RabbitMQService _rabbitMQService;
        private readonly IConfiguration _configuration;
        private readonly ValidationSettings _validationSettings;
        public UserController(ILogger<UserController> logger, IUserRepository userRepository, RabbitMQService rabbitMQService, IConfiguration configuration)
        {
            _logger = logger;
            _userRepository = userRepository;
            _rabbitMQService = rabbitMQService;
            _configuration = configuration;
            _validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new string[] { _configuration["GOOGLE_CLIENT_ID"] }
            };
        }

        [HttpPut("username")]
        public async Task<IActionResult> RegisterAsync([FromBody] User user, [FromHeader] string token)
        {


            Payload? payload = await GoogleJsonWebSignature.ValidateAsync(token, _validationSettings);

            if (user.Name == null) throw new Exception("user.name is required");
            _logger.Log(LogLevel.Information, $"Updated user username to {user.Name}");
            var editedUser = await _userRepository.RegisterOrUpdateUser(payload.Email, user.Name);
            _rabbitMQService.SendMessage(editedUser, "user", "edit");
            try
            {
                //Attempts to try to send the user an welcome email
                var response = await client.PostAsync($"https://s6emailservice.azurewebsites.net/api/emailservice?to={editedUser.Email}&token={_configuration["EmailToken"]}",null);
            }
            catch
            {
                //do nothing
            }
            return Ok(editedUser);
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me([FromHeader] string token)
        {
            Payload? payload = await GoogleJsonWebSignature.ValidateAsync(token, _validationSettings);

            if (token == null) throw new Exception("Call can't be done while the user is not logged in.");
            var user = await _userRepository.Get(payload.Email);
            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string email)
        {
            _logger.LogInformation($"Deleted user {email}");
            await _userRepository.DeleteUser(email);
            return Ok();
        }
    }
}