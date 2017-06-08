//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Dubno.Models;
//using Microsoft.AspNetCore.Hosting;

//namespace Dubno.Controllers
//{
//    public class EmailController : Controller
//    {

//        private IHostingEnvironment _env;

//        public EmailController(IHostingEnvironment env)
//        {
//            _env = env;
//        }

//        public IActionResult SendEmail()
//        {
//            return View();
//        }

//        public IActionResult SentEmail()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<ActionResult> SendEmail(EmailViewModel model)
//        {
//            var emailTemplate = "WelcomeEmail";
//            var emailSubject = "Welcome to our site.";
//            var message = EMailTemplate(emailTemplate);
//            message = message.Replace("@ViewBag.Name", model.Username);
//            MessageServices msg = new MessageServices();
//            await msg.SendEmailAsync(model.Username, model.Email, emailSubject, message, model.Attachments);
//            ModelState.AddModelError("", "Email successfully sent.");
//            return View("EmailSent");
//        }


//        [HttpPost]
//        public async Task<ActionResult> SendBulkEmail(BulkEmailViewModel model)
//        {
//            var emailTemplate = "WelcomeEmail";
//            var emailSubject = "Welcome to our site.";
//            var message = EMailTemplate(emailTemplate);
//            message = message.Replace("@ViewBag.Name", model.Username);
//            MessageServices msg = new MessageServices();
//            await msg.SendBulkEmailAsync(model.Username, model.Email, emailSubject, message, model.Attachments);
//            ModelState.AddModelError("", "Email successfully sent.");
//            return View("BulkEmail");
//        }

//        public string EMailTemplate(string template)
//        {
//            string body = null;
//            var templateFilePath = _env.WebRootPath + "\\templates\\" + template + ".cshtml";
//            body = System.IO.File.ReadAllText(templateFilePath);
//            return body;
//        }
//    }
//}