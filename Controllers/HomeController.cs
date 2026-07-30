using Microsoft.AspNetCore.Mvc;
using Ninel_INFASS2.Models;

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
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (UserRepository.ExistsByUsername(model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            if (UserRepository.ExistsByEmail(model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            UserRepository.Add(model);

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

            var user = UserRepository.GetAll().FirstOrDefault(x =>
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

            if (UserRepository.ExistsByUsername(model.Username))
                return Json(new { success = false, message = "Username already exists." });

            if (UserRepository.ExistsByEmail(model.Email))
                return Json(new { success = false, message = "Email already exists." });

            UserRepository.Add(model);

            return Json(new { success = true, message = "Successfully inserted!" });
        }

        [HttpGet]
        public JsonResult GetUsers()
        {
            var users = UserRepository.GetAll().Select(u => new
            {
                u.Id, u.Name, u.Email, u.Gender, u.Age, u.Address, u.Username
            }).ToList();
            return Json(users);
        }

        [HttpGet]
        public JsonResult GetUser(int id)
        {
            var user = UserRepository.GetById(id);
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
            if (UserRepository.GetById(model.Id) == null)
                return Json(new { success = false, message = "User not found." });

            if (UserRepository.ExistsByUsername(model.Username, model.Id))
                return Json(new { success = false, message = "Username already exists." });

            if (UserRepository.ExistsByEmail(model.Email, model.Id))
                return Json(new { success = false, message = "Email already exists." });

            UserRepository.Update(model);

            return Json(new { success = true, message = "Successfully updated!" });
        }

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            if (!UserRepository.Delete(id))
                return Json(new { success = false, message = "User not found." });

            return Json(new { success = true, message = "Successfully deleted!" });
        }
    }
}
