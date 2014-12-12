using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TimeTracker.Web.Controllers;
using TimeTracker.Model;

namespace TimeTracker.Web.Tests.Controllers
{
    [TestClass]
    public class CustomersControllerTest
    {
        [TestMethod]
        public async Task Get()
        {
            // Arrange
            var context = CreateTestCustomerSet();
            var controller = new CustomersController(context.Object);

            // Act
            IQueryable<Customer> result = controller.GetCustomers();

            // Assert
            Assert.IsNotNull(result);
            var actual = await result.ToListAsync();
            Assert.AreEqual(3, actual.Count);

            var cust = actual.First();
            Assert.IsNotNull(cust);
        }

        private static Mock<ITimeTrackerContext> CreateTestCustomerSet()
        {
            var customers = TestModelData.CreateTestCustomers();
            var customerDbSet = new Mock<DbSet<Customer>>().SetupData(customers, objects => customers[0]);
            var context = new Mock<ITimeTrackerContext>();
            context.Setup(c => c.Customers).Returns(customerDbSet.Object);
            return context;
        }
        
        [TestMethod]
        public async Task GetById()
        {
            // Arrange
            var customers = TestModelData.CreateTestCustomers();
            var customerDbSet = new Mock<DbSet<Customer>>().SetupData(customers, objects => customers[0]);
            customerDbSet.Setup(s => s.FindAsync(2)).ReturnsAsync(customers[1]);
            var context = new Mock<ITimeTrackerContext>();
            context.Setup(c => c.Customers).Returns(customerDbSet.Object);
            var controller = new CustomersController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetCustomer(2);

            // Assert
            Assert.IsNotNull(actionResult);

            var contentResult = actionResult as OkNegotiatedContentResult<Customer>;
            Assert.IsNotNull(contentResult);

            var actualCustomer = contentResult.Content;
            Assert.IsNotNull(actualCustomer);
            Assert.AreEqual(2, actualCustomer.Id);
        }

        [TestMethod]
        public async Task GetByIdReturnsNotFound()
        {
            // Arrange
            var context = CreateTestCustomerSet();
            var controller = new CustomersController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetCustomer(0);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Post()
        {
            // Arrange
            var context = CreateTestCustomerSet();
            var controller = new CustomersController(context.Object);

            var customer10 = new Customer
            {
                Id = 10,
                CompanyName = "Company",
                Address = "Address",
                City = "City",
                ContactEmail = "ContactEmail"
            };

            // Act
            IHttpActionResult actionResult = await controller.PostCustomer(customer10);

            // Assert
            Assert.IsNotNull(actionResult);
            //Assert.IsTrue(actionResult is OkNegotiatedContentResult<Customer>);
        }

        [TestMethod]
        public async Task Put()
        {
            // Arrange
            var context = CreateTestCustomerSet();
            var controller = new CustomersController(context.Object);


            var customer1Updated = new Customer
            {
                Id = 1,
                CompanyName = "CompanyUpdated",
                Address = "AddressUpdated",
                City = "CityUpdated",
                ContactEmail = "ContactEmailUpdated"
            };

            // Act
            IHttpActionResult actionResult = await controller.PutCustomer(1, customer1Updated);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));
        }

        [TestMethod]
        public async Task PutWrongId()
        {
            // Arrange
            var context = CreateTestCustomerSet();
            var controller = new CustomersController(context.Object);


            var customer1Updated = new Customer
            {
                Id = 1,
                CompanyName = "CompanyUpdated",
                Address = "AddressUpdated",
                City = "CityUpdated",
                ContactEmail = "ContactEmailUpdated"
            };

            // Act
            IHttpActionResult actionResult = await controller.PutCustomer(3, customer1Updated);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Delete()
        {
            // Arrange
            var customers = TestModelData.CreateTestCustomers();
            var customerDbSet = new Mock<DbSet<Customer>>().SetupData(customers, objects => customers[0]);
            customerDbSet.Setup(s => s.FindAsync(2)).ReturnsAsync(customers[1]);
            var context = new Mock<ITimeTrackerContext>();
            context.Setup(c => c.Customers).Returns(customerDbSet.Object);
            var controller = new CustomersController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.DeleteCustomer(2);

            // Assert
            var contentResult = actionResult as OkNegotiatedContentResult<Customer>;
            Assert.IsNotNull(contentResult);
        }

        [TestMethod]
        public void GetProjectsForCustomer()
        {

            // Arrange 
            var context = CreateTestCustomerSet();
            var projects = TestModelData.CreateTestProjects();
            var projectsDbSet = new Mock<DbSet<Project>>().SetupData(projects, objects => projects[0]);
            context.Setup(p => p.Projects).Returns(projectsDbSet.Object);
            var controller = new CustomersController(context.Object);

            // Act
            IQueryable<Project> actualProjects = controller.GetProjectsForCustomer(1);

            // Assert
            Assert.IsNotNull(actualProjects);
            Assert.AreEqual(1, actualProjects.AsEnumerable().Count());
        }
    }
}
