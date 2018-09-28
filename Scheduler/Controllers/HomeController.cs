using Scheduler.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Scheduler.Controllers
{
    public class HomeController : Controller
    {
        EventContext ctx = new EventContext();

        // GET: Home
        public ActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> SendEmail()
        {
            var message = EmailTemplate("WelcomeEmail");
            message = message.Replace("", "");
            message = message.Replace("", "");
            await SenEmailAsync("e-mail_to@gmail.com", "Weekly Schedule", message);
            return Json(0, JsonRequestBehavior.AllowGet);

        }
        public string EmailTemplate(string template)
        {
            EventContext ctx = new EventContext();
            try
            {

                DateTime startDate = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
                DateTime thisSunday = startDate.AddDays(6);
                var WeekEvents = ctx.Eventler
                                   .Where(i => (i.Start >= startDate))
                                   .Where(i => (i.End <= thisSunday))
                                   .OrderBy(i => i.Start)
                                   .ToList();

                var mailMessage = "";

                foreach (var item in WeekEvents)
                {
                    mailMessage += item.Subject + " on " + item.Start.DayOfWeek + " at " + item.Start.ToString("HH:mm") + "<hr>";
                }

                System.IO.File.WriteAllText(HostingEnvironment.MapPath("~/Content/templates/") + template + ".cshtml", "");
                System.IO.File.AppendAllText(HostingEnvironment.MapPath("~/Content/templates/") + template + ".cshtml", mailMessage);
                string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Content/templates/") + template + ".cshtml");
                return body.ToString();
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public async static Task SenEmailAsync(string email, string subject, string message)
        {
            try
            {
                var _email = "e-mail_from@gmail.com";
                var _epass = ConfigurationManager.AppSettings["EmailPassword"];
                var _dispName = "Scheduler App  ";
                MailMessage myMessage = new MailMessage();
                myMessage.To.Add(email);
                myMessage.From = new MailAddress(_email, _dispName);
                myMessage.Subject = subject;
                myMessage.Body = message;
                myMessage.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_email, _epass);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(myMessage);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult AddEvent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEvent(Event e)
        {
            if (ModelState != null)
            {
                try
                {
                    if (e.AmountWeek != null)
                    {

                        for (int i = 0; i <= e.AmountWeek * 7; i = i + 7)
                        {
                            if (i == 0)
                            {
                                ctx.Eventler.Add(e);
                                ctx.SaveChanges();

                                continue;
                            }
                            
                            ctx.Eventler.Add(e);
                            ctx.SaveChanges();
                        }

                    }
                    else
                    {
                        
                        e.ThemeColor = "#286090";

                        ctx.Eventler.Add(e);
                        ctx.SaveChanges();

                    }
                }
                catch (Exception)
                {
                    throw new Exception();

                }
                TempData["SuccessAdd"] = e;
            }

            return View(e);
        }

        public JsonResult GetEvents()
        {
            var events = ctx.Eventler.ToList();
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult DeleteEvent(int Id)
        {
            var status = false;
            var selectEvent = ctx.Eventler.Where(i => i.Id == Id).FirstOrDefault();

            if (selectEvent != null)
            {

                ctx.Eventler.Remove(selectEvent);
                ctx.SaveChanges();
                status = true;
            };

            return new JsonResult { Data = new { status = status } };

        }
    }
}