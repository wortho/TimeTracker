using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TimeTracker.Model
{
    public class ApplicationUser : IdentityUser
    {

        public virtual ICollection<TimeEntry>TimeEntries { get; set; }

    }
}