using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Owin;
using Owin;
using Scheduler.Controllers;

[assembly: OwinStartup(typeof(Scheduler.Startup1))]

namespace Scheduler
{
    public partial class Startup1
    {
        public void Configuration(IAppBuilder app)
        {

            GlobalConfiguration.Configuration.UseSqlServerStorage("EventDb");
            app.UseHangfireDashboard("/myJobDashboard");
            HomeController obj = new HomeController();
            //RecurringJob.AddOrUpdate(() => obj.SendEmail(), Cron.Minutely);
            RecurringJob.AddOrUpdate(() => obj.SendEmail(), "*/10 * * * *");
            app.UseHangfireServer();
        }
    }
}
