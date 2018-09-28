using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class EventContext:DbContext
    {
        public EventContext() : base("EventDb")
        {
            Database.SetInitializer(new BlogInitializer());
        }
        public DbSet<Event> Eventler { get; set; }
    }
}