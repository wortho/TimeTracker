using System.Data.Entity;
using TimeTracker.Web.Models;

namespace TimeTracker.Web.Repository
{
    public class TimeTrackerContext: DbContext, ITimeTrackerContext
    {
        public TimeTrackerContext()
            : base("DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        // DEVELOPMENT ONLY: initialize the database
        static TimeTrackerContext()
        {
            Database.SetInitializer(new TimeTrackerDatabaseInitializer());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

    }
}