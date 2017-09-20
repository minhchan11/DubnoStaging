using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dubno.Models;
using Dubno.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;

namespace Dubno.Controllers
{
    public class HomeController : Controller
    {

        private IHostingEnvironment _environment;


        public HomeController(IHostingEnvironment environment)
        {

            _environment = environment;
        }


        //creates a new instance of database as an entension of the DubnoDbContext class
        private DubnoDbContext db = new DubnoDbContext();


        public IActionResult Index()
        {
            ViewData["FeaturedPosts"] = db.Posts.OrderByDescending(a => a.PostDate).ToList().Take(3);


            //returns the view with a list of all the approved posts descending by post date which updates automatically at different times (initial, edits, and post)
            return View(db.Posts.OrderByDescending(a => a.PostDate).ToList());
        }


        public IActionResult About()
        {
            //returns the view for about page
            return View();
        }

        [HttpPost]
        public IActionResult About(Subscriber subscriber)
        {
            //adds subscriber to subscribers database
            db.Subscribers.Add(subscriber);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult AdminView()
        {
            
        
            //returns the view with a list of all unapproved posts
            return View(db.Posts.OrderByDescending(a => a.PostDate).ToList());
        }


        public IActionResult Details(int id)
        {
            //returns view with the specific postId 
            //displays featured posts on details page for users, but not for admin
            ViewData["FeaturedPosts"] = db.Posts.OrderByDescending(a => a.PostDate).ToList().Take(4);

            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);

            return View(thisPost);
        }


        public IActionResult SuggestPost()
        {
            //returns view to allow users to submit posts 

            return View(new PostViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Upload(PostViewModel newPost, IFormFile image)
        {
            //creates a new instance of a post. Takes photo as btye into database. Then sends email to confirm that the user has submitted a post.

            var post = new Post { Description = newPost.Description, Title = newPost.Title, Name = newPost.Name, Email = newPost.Email, City = newPost.City, State = newPost.State };
            byte[] profilePic = ConvertToBytes(image);
            post.ImageName = profilePic;
            post.Approved = false;
            post.Pending = true;
            post.PostDate = DateTime.Now;

            //sends the email

            var emailMessage = new MimeMessage();


            emailMessage.From.Add(new MailboxAddress("The Peace Corps", "humansofdubno@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(newPost.Name, newPost.Email));
            emailMessage.Subject = "Post Submission";
            emailMessage.Body = new TextPart("html")
            {
                Text = string.Format("Dear " + newPost.Name + "<br/> Thank you for your contribution to Humans of Dubno. We hace received you submission and it is under review. You will recieve another email if any edits are made or if your post has been selected to display. <br> Thank you, <br> Humans of Dubno Staff")
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("humansofdubno@gmail.com", "dubno2017");
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            };

            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            //returns edit page with selected post information pre loaded
            var post = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(post);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Post post, IFormFile avatar)
        {
            //allows admin to edit a post. It is important to note that in order for the edit to go through the admin MUST re upload the photo. Will work on seperating these two in the future, and adding in house photo editing.
          
            var editedPost = db.Posts.FirstOrDefault(tourists => tourists.PostId == post.PostId);
            db.Posts.Attach(editedPost);
            byte[] profilePic = ConvertToBytes(avatar);
            editedPost.Name = post.Name;
            editedPost.City = post.City;
            editedPost.State = post.State;
            editedPost.Description = post.Description;
            editedPost.Title = post.Title;
            editedPost.Email = post.Email;
            editedPost.AdminComment = post.AdminComment;

            editedPost.PostDate = DateTime.Now;
            editedPost.Approved = false;
            editedPost.Pending = true;

            editedPost.ImageName = profilePic;
            db.SaveChanges();
            

            //sends email after edits have been saved with details of what edits have been made. MUST SEND AN ADMIN COMMENT
            var emailMessage = new MimeMessage();


            emailMessage.From.Add(new MailboxAddress("Keely", "keelyzglenn@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(post.Name, post.Email));
            emailMessage.Subject = "Post Edit";
            emailMessage.Body = new TextPart("html")
            {
                Text = string.Format("Dear " + post.Name + "<br/> Some edits have been made to your submitted post. Please review the admin comments below for further information.<br>" + post.AdminComment + "<br> Thank you, <br> Humans of Dubno Staff")
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("keelyzglenn@gmail.com", "monkey1963");
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            };

            return RedirectToAction("AdminView");
        }


        [HttpPost]
        public async Task<IActionResult> ApprovePost(int id)
        {

            //changes the post booleans to true and false allowing it to be viewed by users. Sends email when approved. 
            var thisPost = db.Posts.FirstOrDefault(i => i.PostId == id);

            var emailMessage = new MimeMessage();


            emailMessage.From.Add(new MailboxAddress("Humans of Dubno", "humansofdubno@gmail.com"));
            emailMessage.To.Add(new MailboxAddress(thisPost.Name, thisPost.Email));
            emailMessage.Subject = "Post Submission";
            emailMessage.Body = new TextPart("html")
            {
                Text = string.Format("Dear " + thisPost.Name + "<br/> Thank you for your contribution to Humans of Dubno. You post has been selected to display on the webpage. <br> Thank you, <br> Humans of Dubno Staff")
            };


            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("humansofdubno@gmail.com", "dubno2017");
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            };

            thisPost.Approved = true;
            thisPost.Pending = false;
            thisPost.PostDate = DateTime.Now;
            db.Entry(thisPost).State = EntityState.Modified;
            db.SaveChanges();
            //uses ajax to automaticially update the page
            return Json(thisPost);
        }

        public IActionResult Delete(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            return View(thisPost);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            //will delete the post from database
            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);
            db.Posts.Remove(thisPost);
            db.SaveChanges();
            return RedirectToAction("AdminView");
        }


        private byte[] ConvertToBytes(IFormFile image)
        {
            //converts image to btyes to be saved in database
            byte[] CoverImageBytes = null;
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            CoverImageBytes = reader.ReadBytes((int)image.Length);
            return CoverImageBytes;
        }


    }

}