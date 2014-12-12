using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace TimeTracker.Model
{
    public interface ITimeTrackerContext : IDisposable
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<TimeEntry> TimeEntries { get; set; }
        Task<int> SaveChangesAsync();
        DbEntityEntry Entry(object entity);
        void SetModified(object entity);

    }
}