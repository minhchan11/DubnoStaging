using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dubno.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Dubno.Controllers
{
    public class EmailController : Controller
    {

        private IHostingEnvironment _env;

        public EmailController(IHostingEnvironment env)
        {
            _env = env;
        }

        private DubnoDbContext db = new DubnoDbContext();

     

        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailViewModel model)
        {
           
                var emailMessage = new MimeMessage();


                emailMessage.From.Add(new MailboxAddress("Keely", "keelyzglenn@gmail.com"));
                emailMessage.To.Add(new MailboxAddress("keely", "keelyzglenn@gmail.com"));
                emailMessage.Subject = "Email confirmation";
                emailMessage.Body = new TextPart("html")
                {
                    Text = string.Format("Dear <br/> Thank you for your registration, please click on the below link to complete your registration",
                model.Username, Url.Action("SentEmail", "Email"))
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    client.Authenticate("keelyzglenn@gmail.com", "monkey1963");
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                };


            return RedirectToAction("Sent", "Email");
            }

        public async Task<IActionResult> Index(int page = 1)
        {
            var qry = db.Posts.AsNoTracking().OrderBy(p => p.Title);
            var model = await PagingList<Post>.CreateAsync(qry, 3, page);
            return View(model);
        }

        public IActionResult SendEmail()
        {
            return View();
        }


        //[HttpPost]
        //public async Task<ActionResult> SendEmail(EmailViewModel model)
        //{
        //    var emailTemplate = "WelcomeEmail";
        //    var emailSubject = "Welcome to our site";
        //    var message = EMailTemplate(emailTemplate);
        //    message = message.Replace("@ViewBag.Name", model.Username);
        //    MessageServices msg = new MessageServices();
        //    await msg.SendEmailAsync(model.Username, model.Email, emailSubject, message, model.Attachments);
        //    ModelState.AddModelError("", "Email successfully sent.");
        //    return View("EmailSent");
        //}


        public IActionResult SentEmail()
        {
            return View();
        }


        public string EMailTemplate(string template)
        {
            string body = null;
            var templateFilePath = _env.WebRootPath + "\\templates\\" + template + ".cshtml";
            body = System.IO.File.ReadAllText(templateFilePath);
            return body;
        }
    }
}