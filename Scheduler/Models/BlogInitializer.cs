using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Scheduler.Models
{
    public class BlogInitializer : DropCreateDatabaseIfModelChanges<EventContext>
    {
        protected override void Seed(EventContext context)
        {
            EventContext db = new EventContext();

            List<Event> eventler = new List<Event>()
            {
                //new Event() {Subject="Okulun başlama tarihi",Description=null,Start=DateTime.Now.AddDays(5),End=null,IsFullDay=false},
                //new Event() {Subject="Cemrenin Doğum günü",Description=null,Start=DateTime.Now.AddDays(24),End=null,IsFullDay=false,ThemeColor="#1c0680"},
                // new Event() {Subject="Ders Seçme",Description="Ders Seçim Haftası",Start=DateTime.Now.AddDays(-3),End=DateTime.Now.AddDays(3),IsFullDay=false,ThemeColor="#800665"}
            };
            foreach (var item in eventler)
            {
                db.Eventler.Add(item);
            }
            db.SaveChanges();

            base.Seed(context);
        }
    }
}