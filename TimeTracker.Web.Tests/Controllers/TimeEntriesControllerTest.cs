using System;
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
    public class TimeEntriesControllerTest
    {
        [TestMethod]
        public async Task Get()
        {
            // Arrange
            var context = CreateTestContextMock();
            var controller = new TimeEntriesController(context.Object);

            // Act
            IQueryable<TimeEntry> result = controller.GetTimeEntries();

            // Assert
            Assert.IsNotNull(result);
            var actual = await result.ToListAsync();
            Assert.AreEqual(9, actual.Count);

            var cust = actual.First();
            Assert.IsNotNull(cust);
        }

        private static Mock<ITimeTrackerContext> CreateTestContextMock()
        {
            var context = new Mock<ITimeTrackerContext>();
            var projects = TestModelData.CreateTestProjects();
            var timeEntries = TestModelData.CreateTimeEntriesForProjects(projects);
            var timeEntryDbSet = new Mock<DbSet<TimeEntry>>().SetupData(timeEntries, objects => timeEntries[0]);
            context.Setup(c => c.TimeEntries).Returns(timeEntryDbSet.Object);
            return context;
        }

        [TestMethod]
        public async Task GetById()
        {
            // Arrange
            var context = new Mock<ITimeTrackerContext>();
            var projects = TestModelData.CreateTestProjects();
            var timeEntries = TestModelData.CreateTimeEntriesForProjects(projects);
            var timeEntryDbSet = new Mock<DbSet<TimeEntry>>().SetupData(timeEntries, objects => timeEntries[0]);
            timeEntryDbSet.Setup(t => t.FindAsync(1102)).ReturnsAsync(timeEntries[1]);
            context.Setup(c => c.TimeEntries).Returns(timeEntryDbSet.Object);
            var controller = new TimeEntriesController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetTimeEntry(1102);

            // Assert
            Assert.IsNotNull(actionResult);

            var contentResult = actionResult as OkNegotiatedContentResult<TimeEntry>;
            Assert.IsNotNull(contentResult);

            var actual = contentResult.Content;
            Assert.IsNotNull(actual);
            Assert.AreEqual(1102, actual.Id);
        }

        [TestMethod]
        public async Task GetByIdReturnsNotFound()
        {
            // Arrange
            var context = CreateTestContextMock();
            var controller = new TimeEntriesController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetTimeEntry(1010);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Post()
        {
            // Arrange
            var context = CreateTestContextMock();
            var controller = new TimeEntriesController(context.Object);

            var project22 = new TimeEntry
            {
                Id = 220,
                ProjectId = 22,
                Description = "Test",
                User = new ApplicationUser() {UserName = "Test1"}
            };

            // Act
            IHttpActionResult actionResult = await controller.PostTimeEntry(project22);

            // Assert
            Assert.IsNotNull(actionResult);
            //Assert.IsTrue(actionResult is OkNegotiatedContentResult<Customer>);
        }


        [TestMethod]
        public async Task PutWrongId()
        {
            // Arrange
            var context = CreateTestContextMock();
            var controller = new TimeEntriesController(context.Object);


            var project11Update = new TimeEntry
            {
                Id = 110,
                ProjectId = 11,
                Description = "Project11Updated",
                User = new ApplicationUser() {UserName = "Test1"}
            };

            // Act
            IHttpActionResult actionResult = await controller.PutTimeEntry(21, project11Update);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task Put()
        {
            // Arrange
            var context = CreateTestContextMock();
            var controller = new TimeEntriesController(context.Object);


            var entry = new TimeEntry
            {
                Id = 1100,
                ProjectId = 11,
                Description = "1100Updated",
                User = new ApplicationUser() {UserName = "Test1"}
            };

            // Act
            IHttpActionResult actionResult = await controller.PutTimeEntry(1100, entry);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));
        }

        [TestMethod]
        public async Task PutNonExistentId()
        {
            // Arrange
            var context = CreateTestContextMock();
            var controller = new TimeEntriesController(context.Object);


            var entry = new TimeEntry
            {
                Id = 42,
                ProjectId = 11,
                Description = "1100Updated",
                User = new ApplicationUser() { UserName = "Test1" }
            };

            // Act
            IHttpActionResult actionResult = await controller.PutTimeEntry(42, entry);

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult, typeof(StatusCodeResult));
        }

        [TestMethod]
        public async Task Delete()
        {
            // Arrange
            var projects = TestModelData.CreateTestProjects();
            var timeEntries = TestModelData.CreateTimeEntriesForProjects(projects);
            var timeEntryDbSet = new Mock<DbSet<TimeEntry>>().SetupData(timeEntries, objects => timeEntries[0]);
            timeEntryDbSet.Setup(t => t.FindAsync(1103)).ReturnsAsync(timeEntries[2]);
            var context = new Mock<ITimeTrackerContext>();
            context.Setup(c => c.TimeEntries).Returns(timeEntryDbSet.Object);
            var controller = new TimeEntriesController(context.Object);

            // Act
            IHttpActionResult actionResult = await controller.DeleteTimeEntry(1103);

            // Assert
            var contentResult = actionResult as OkNegotiatedContentResult<TimeEntry>;
            Assert.IsNotNull(contentResult);
        }

    }
}
