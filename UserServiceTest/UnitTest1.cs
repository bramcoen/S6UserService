using DataInterface;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using UserService.Controllers;

namespace UserServiceTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFlow()
        {
            var mock = new Mock<IUserRepository>();
            User user = new User("Bella Mongomary") { Email = "Test@test.com"};
            mock.Setup(library => library.SaveUserAsync(user)).ReturnsAsync(user);
            IUserRepository userRepository = mock.Object;
            UserController userController = new(NullLogger<UserController>.Instance, userRepository);
            userController.Register("Test@test.com", "Bella Mongomary");
            mock.Verify(library => library.SaveUserAsync(user), Times.Once());
        }
    }
}