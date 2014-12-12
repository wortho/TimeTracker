using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TimeTracker.Model;

namespace TimeTracker.Repository.Tests
{
    [TestClass]
    public class TimeTrackerDatabaseInitializerTest
    {
        [TestMethod]
        public void CreateTimeTrackerDatabaseInitializer()
        {
            var initializer = new TimeTrackerDatabaseInitializer();
            Assert.IsNotNull(initializer);
        }

        [TestMethod]
        public void Generate()
        {
            // Arrange
            var context = new Mock<ITimeTrackerContext>();
            var customerDbSet = new Mock<DbSet<Customer>>().SetupData(new List<Customer>());
            context.Setup(c => c.Customers).Returns(customerDbSet.Object);

            var projectsDbSet = new Mock<DbSet<Project>>().SetupData(new List<Project>());
            context.Setup(p => p.Projects).Returns(projectsDbSet.Object);

            // Act
            TimeTrackerDatabaseInitializer.GenerateSeedData(context.Object);

            //Assert
            Assert.AreEqual<int>(6, context.Object.Customers.Count());
            Assert.AreEqual<int>(24, context.Object.Projects.Count());
        }


    }
}
