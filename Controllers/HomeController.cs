using Microsoft.AspNetCore.Mvc;
using Ninel_INFASS2.Models;
using System.Linq;

namespace Ninel_INFASS2.Controllers
{
    public class HomeController : Controller
    {
        private static int _nextId = 1;

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

        //================ AJAX CRUD ================

        [HttpPost]
        public JsonResult RegisterAjax([FromBody] RegisterModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Gender) || string.IsNullOrEmpty(model.Address) ||
                string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return Json(new { success = false, message = "All fields are required." });
            }

            if (UserRepository.Users.Any(x => x.Username == model.Username))
                return Json(new { success = false, message = "Username already exists." });

            if (UserRepository.Users.Any(x => x.Email == model.Email))
                return Json(new { success = false, message = "Email already exists." });

            model.Id = _nextId++;
            UserRepository.Users.Add(model);

            return Json(new { success = true, message = "Successfully inserted!" });
        }

        [HttpGet]
        public JsonResult GetUsers()
        {
            var users = UserRepository.Users.Select(u => new
            {
                u.Id, u.Name, u.Email, u.Gender, u.Age, u.Address, u.Username
            }).ToList();
            return Json(users);
        }

        [HttpGet]
        public JsonResult GetUser(int id)
        {
            var user = UserRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            return Json(new
            {
                user.Id, user.Name, user.Email, user.Gender, user.Age, user.Address, user.Username
            });
        }

        [HttpPost]
        public JsonResult UpdateUser([FromBody] RegisterModel model)
        {
            var user = UserRepository.Users.FirstOrDefault(u => u.Id == model.Id);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            if (UserRepository.Users.Any(x => x.Username == model.Username && x.Id != model.Id))
                return Json(new { success = false, message = "Username already exists." });

            if (UserRepository.Users.Any(x => x.Email == model.Email && x.Id != model.Id))
                return Json(new { success = false, message = "Email already exists." });

            user.Name = model.Name;
            user.Email = model.Email;
            user.Gender = model.Gender;
            user.Age = model.Age;
            user.Address = model.Address;
            user.Username = model.Username;
            if (!string.IsNullOrEmpty(model.Password))
                user.Password = model.Password;

            return Json(new { success = true, message = "Successfully updated!" });
        }

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            var user = UserRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            UserRepository.Users.Remove(user);
            return Json(new { success = true, message = "Successfully deleted!" });
        }
    }
}
