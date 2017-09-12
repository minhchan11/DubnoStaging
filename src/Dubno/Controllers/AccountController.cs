using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Dubno.Models;
using Dubno.ViewModels;


namespace Dubno.Controllers
{
    public class AccountController : Controller
    {
        private DubnoDbContext db = new DubnoDbContext();

        private readonly DubnoDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DubnoDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }


        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var thisUser = db.Users.FirstOrDefault(item => item.UserName == User.Identity.Name);
                return View(thisUser);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("AdminView", "Home");
            }
            else
            {
                ViewBag.Error = "No match for username and password";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}