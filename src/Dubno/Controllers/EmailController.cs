//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Dubno.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using ReflectionIT.Mvc.Paging;

//namespace Dubno.Controllers
//{
//    public class EmailController : Controller
//    {

//        private IHostingEnvironment _env;

//        public EmailController(IHostingEnvironment env)
//        {
//            _env = env;
//        }

//        private DubnoDbContext db = new DubnoDbContext();

//        public async Task<IActionResult> Index(int page = 1)
//        {
//            var qry = db.Posts.AsNoTracking().OrderBy(p => p.Title);
//            var model = await PagingList<Post>.CreateAsync(qry, 3, page);
//            return View(model);
//        }

//        public IActionResult SendEmail()
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


//        //public IActionResult SentEmail()
//        //{
//        //    return View();
//        //}


//        public string EMailTemplate(string template)
//        {
//            string body = null;
//            var templateFilePath = _env.WebRootPath + "\\templates\\" + template + ".cshtml";
//            body = System.IO.File.ReadAllText(templateFilePath);
//            return body;
//        }
//    }
//}