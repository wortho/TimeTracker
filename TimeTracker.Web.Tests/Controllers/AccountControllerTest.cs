using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker.Web;
using TimeTracker.Web.Controllers;

namespace TimeTracker.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void AccountController()
        {
            // Arrange Act
            AccountController controller = new AccountController();
            
            // Assert
            Assert.IsNotNull(controller);
        }
    }
}
