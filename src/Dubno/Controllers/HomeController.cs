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
            //returns the view with a list of all the approved posts
            return View(db.Posts.OrderBy(s => s.PostDate).ToList());
        }


        public IActionResult AdminView()
        {
            //returns the view with a list of all unapproved posts
            return View(db.Posts.OrderBy(s => s.PostDate).ToList());
        }
        public IActionResult Details(int id)
        {
            //returns view with the specific postId 

            var thisPost = db.Posts.FirstOrDefault(posts => posts.PostId == id);

            var nextID = db.Posts.OrderBy(i => i.PostId)
                     .SkipWhile(i => i.PostId != id)
                     .Skip(1)
                     .Select(i => i.PostId);
            ViewBag.NextID = nextID;

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
            //this method will allow the admin to edit the post
            post.PostDate = DateTime.Now;
            post.Approved = false;
            post.Pending = true;
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AdminView");
        }

        //public IActionResult ApprovePost(int id )
        //{
        //    var thisPost = db.Posts.FirstOrDefault(i => i.PostId == id);
        //    Console.WriteLine(thisPost);
        //    return View(thisPost);
        //}

        [HttpPost]
        public IActionResult ApprovePost(int id)
        {
            var thisPost = db.Posts.FirstOrDefault(i => i.PostId == id);
            Console.WriteLine(thisPost.Approved);
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
            return RedirectToAction("Index");
        }

        

    }

}