using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dubno.Models;


namespace Dubno.Controllers
{
    public class HomeController : Controller
    {
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
            return View(thisPost);
        }

        public IActionResult SuggestPost()
        {
          
            return View();
        }

        [HttpPost]
        public IActionResult SuggestPost(Post post)
        {

            //when users submit a suggested post, they will create an instance of that post in the database, the method will automatically set its pending status to true, which will allow the Admin to see the new post in their dashboard, it will also automatically set the approved to denied, therefore preventing it from being shown on the posts page. 
            post.Approved = false;
            post.PostDate = DateTime.Now;
            post.Pending = true;
            db.Posts.Add(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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


        [HttpPost]
        public IActionResult ApprovePost(int id)
        {
            //when an admin approves a post it will set the posts approve property to true, and the pending property to false- therefore allowing it to show on the page
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
            return RedirectToAction("Index");
        }
    }
}