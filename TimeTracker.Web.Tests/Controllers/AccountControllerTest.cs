using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TimeTracker.Model;
using TimeTracker.Web.Controllers;

namespace TimeTracker.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void AccountController()
        {
            // Arrange
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var manager = new ApplicationUserManager(userStore.Object);

            // Act
            var controller = new AccountController(manager, accessTokenFormat: null);

            // Assert
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public async Task Register()
        {
            // Arrange
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var passwordStore = userStore.As<IUserPasswordStore<ApplicationUser>>();
            userStore.Setup(store => store.CreateAsync(It.IsAny<ApplicationUser>())).Returns(Task.FromResult(new IdentityResult(new string[] { })));
            var manager = new ApplicationUserManager(userStore.Object);
            var controller = new AccountController(manager, accessTokenFormat: null);
            var model = new RegisterBindingModel
            {
                Email = "test@nowhere.com",
                Password = "Password1",
                ConfirmPassword = "Password1"
            };

            // Act
            var result = await controller.Register(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is OkResult);

        }

    }
}
