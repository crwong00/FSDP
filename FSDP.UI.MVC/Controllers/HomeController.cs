using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System;
using FSDP.UI.MVC.Models;


namespace FSDP.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(bool? complete)
        {
            string currentUserID = User.Identity.GetUserId();

            ViewBag.id = currentUserID;

            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactViewModel cvm)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(cvm);
            //}
                // 1) Build the email message body (content for the email)
                string message = $"{cvm.Name} has sent you a message, Please respond to " +
                    $"<cite>{cvm.Email}</cite> <br> <cite>{cvm.Message}</cite>";

                // 2) Create the MailMessage object, and customize
                MailMessage msg = new MailMessage(
                    //FROM - your domain email (admin@yourdomain.com)
                    "admin@thewongpage.com",
                    //TO - where the email lands (should be sent to your personal email)
                    "crwong00@outlook.com",
                    //subject
                    "Help!",
                    //Body
                    message
                    );

                //allow HTML formatting
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;//cna change priority
                                                 //CC  or BCC other recipients


                // 3) Create the SmtpClient that will send the email.
                //the client will need info from the host to route the email
                SmtpClient client = new SmtpClient("mail.TheWongpage.com");
                client.Credentials = new NetworkCredential("admin@TheWongPage.com", "Smokes#1");


                // 4) Attempt sending email.
                try
                {
                    client.Send(msg);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Sorry, something went wrong. Pelase try again later or review the stacktrace <br>{ex.StackTrace}";
                    return View();
                }

                
                return RedirectToAction("TileView", "Cours");
            
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Contact()
        //{

        //    return View();

        //}
    }
}
