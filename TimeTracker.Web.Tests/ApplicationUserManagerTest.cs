using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TimeTracker.Model;

namespace TimeTracker.Web.Tests
{
    [TestClass]
    public class ApplicationUserManagerTest
    {
        [TestMethod]
        public void ApplicationUserManager()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var manager = new ApplicationUserManager(userStore.Object);

            Assert.IsNotNull(manager);
        }
    }
}
