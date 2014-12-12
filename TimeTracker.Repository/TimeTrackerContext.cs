using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeTracker.Model;

namespace TimeTracker.Repository
{
    public class TimeTrackerContext: IdentityDbContext<ApplicationUser>, ITimeTrackerContext
    {
        public TimeTrackerContext(string connectionString)
            : base(connectionString)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            
            // enable SQL logging
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public static TimeTrackerContext Create()
        {
            return new TimeTrackerContext("DefaultConnection");
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // use the AspNetIdentity tables for the User
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");

            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");
            
            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey }).ToTable("AspNetUserLogins");

            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId }).ToTable("AspNetUserRoles");

            modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

        }

    }
}