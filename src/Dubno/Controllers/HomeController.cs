using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dubno.Models;
using Dubno.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;


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


            //returns the view with a list of all the approved posts
            return View(db.Posts.OrderByDescending(a => a.PostDate).ToList());
        }

        public IActionResult About()
        {
            //returns the view for about page where users can subscribe to get emails
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
            ViewData["FeaturedPosts"] = db.Posts.OrderByDescending(a => a.PostDate).ToList().Take(4);

            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);

            return View(thisPost);
        }


        public IActionResult SuggestPost()
        {
            return View(new PostViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(PostViewModel newPost)
        {
            var fileName = "";
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in newPost.files)
            {
                if (file.Length > 0)
                {
                    fileName = file.FileName;
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            var post = new Post { ImageName = "/uploads/" + fileName, Description = newPost.Description, Title = newPost.Title, Name = newPost.Name, Email = newPost.Email, City = newPost.City, State = newPost.State };
            post.Approved = false;
            post.Pending = true;
            post.PostDate = DateTime.Now;
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //end handling new content


        public IActionResult Edit(int id)
        {
            var thisPlace = db.Posts.FirstOrDefault(places => places.PostId == id);
            return View(thisPlace);
        }

        [HttpPost]
        public IActionResult Edit(Post post)
        {
            post.PostDate = DateTime.Now;
            post.Approved = false;
            post.Pending = true;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AdminView");
        }


        [HttpPost]
        public IActionResult ApprovePost(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(i => i.PostId == id);
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

    }

}