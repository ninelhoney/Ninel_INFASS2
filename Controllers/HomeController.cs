using Microsoft.AspNetCore.Mvc;
using Ninel_INFASS2.Models;
using System.Linq;

namespace Ninel_INFASS2.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        //================ REGISTER ================

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (UserRepository.Users.Any(x => x.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            if (UserRepository.Users.Any(x => x.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            UserRepository.Users.Add(model);

            TempData["Success"] = "Registration Successful! Please login.";

            return RedirectToAction("Login");
        }

        //================ LOGIN ================

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = UserRepository.Users.FirstOrDefault(x =>
                x.Username == model.Username &&
                x.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid Username or Password.";
                return View(model);
            }

            TempData["User"] = user.Name;

            return RedirectToAction("Index");
        }
    }
}