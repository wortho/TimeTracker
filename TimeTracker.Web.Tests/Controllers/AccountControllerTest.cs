using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker.Web.Controllers;

namespace TimeTracker.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        
        [TestMethod]
        public async Task GetUserInfo()
        {
            await RunWithTestServer("/api/Account/UserInfo", HttpStatusCode.Unauthorized, async response =>
            {
                var returnValue = await response.Content.ReadAsAsync<object>();
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, returnValue.ToString());

            });
        }

        //[TestMethod]
        //public async Task Register()
        //{

        //    // Arrange
        //    var model = new RegisterBindingModel()
        //        {
        //            Email = "test@nowhere.com",
        //            Password = "P@ssword1",
        //            ConfirmPassword = "P@ssword1"
        //        };

         
        //    await RunWithTestServer(async server =>
        //     {
        //         // Act
        //         var response = await server.HttpClient.PostAsJsonAsync("api/Account/Register", model);

        //         // Assert
        //         Assert.IsNotNull(response);
        //         var returnValue = await response.Content.ReadAsAsync<object>();
        //         Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, returnValue.ToString());
        //     });
        //}


        private static async Task RunWithTestServer(Func<TestServer, Task> test)
        {
            using (var server = TestServer.Create(builder =>
            {
                var startup = new Startup();
                builder.Properties.Add("Test", true);
                startup.Configuration(builder);
            }))
            {
                await test(server);
            }
        }
        

        private static async Task RunWithTestServer(string route, HttpStatusCode expectedStatusCode, Func<HttpResponseMessage, Task> validateResponse = null)
        {
            await RunWithTestServer(async server =>
            {
                var response = await server.HttpClient.GetAsync(route);
                Assert.IsNotNull(response);
                Assert.AreEqual(expectedStatusCode, response.StatusCode, response.ReasonPhrase);
                if (validateResponse != null) await validateResponse(response);
            });
        }
    }
}
