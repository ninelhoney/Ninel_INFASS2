using Microsoft.AspNetCore.Mvc;
using Ninel_INFASS2.Models;

namespace Ninel_INFASS2.Controllers
{
    public class AccountController : Controller
    {
        private static List<UserData> _users = new List<UserData>
        {
            new UserData { Id = 1, FullName = "Juan Dela Cruz", Email = "juan@example.com", Gender = "Male", Age = 25, Address = "Manila, Philippines", Username = "juancruz", Password = "pass123" },
        };
        private static int _nextId = 2;

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Home/Login.cshtml", model);

            var user = _users.FirstOrDefault(u =>
                u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View("~/Views/Home/Login.cshtml", model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml");
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill in all required fields.";
                ViewBag.RegName = model.FullName;
                ViewBag.RegEmail = model.Email;
                ViewBag.RegGender = model.Gender;
                ViewBag.RegAge = model.Age;
                ViewBag.RegAddress = model.Address;
                ViewBag.RegUsername = model.Username;
                return View("~/Views/Home/Register.cshtml", model);
            }

            var user = new UserData
            {
                Id = _nextId++,
                FullName = model.FullName,
                Email = model.Email,
                Gender = model.Gender,
                Age = model.Age,
                Address = model.Address,
                Username = model.Username,
                Password = model.Password
            };

            _users.Add(user);

            var sqlHelper = new User();
            string[] fields = { "FullName", "Email", "Gender", "Age", "Address", "Username", "Password" };
            string[] values = { model.FullName, model.Email, model.Gender, model.Age.ToString(), model.Address, model.Username, model.Password };
            string sql = sqlHelper.SqlInsert(fields, values, "Users");

            return Json(new { success = true, message = "User registered successfully.", user, sql });
        }

        [HttpPost("Update")]
        public IActionResult Update([FromBody] RegisterViewModel model, int id)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Age must be between 15 and 100." });

            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
                return Json(new { success = false, message = "User not found." });

            existing.FullName = model.FullName;
            existing.Email = model.Email;
            existing.Gender = model.Gender;
            existing.Age = model.Age;
            existing.Address = model.Address;
            existing.Username = model.Username;
            existing.Password = string.IsNullOrEmpty(model.Password) ? existing.Password : model.Password;

            return Json(new { success = true, message = "User updated successfully.", user = existing });
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            var existing = _users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
                return Json(new { success = false, message = "User not found." });

            _users.Remove(existing);

            return Json(new { success = true, message = "User deleted successfully." });
        }

        [HttpGet("ViewAll")]
        public IActionResult ViewAll()
        {
            return Json(_users);
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            return Json(user);
        }

        [HttpPost("CheckLogin")]
        public IActionResult CheckLogin([FromBody] LoginViewModel model)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username == model.Username && u.Password == model.Password);

            if (user == null)
                return Json(new { success = false, message = "Invalid username or password." });

            return Json(new { success = true, message = "Login successful.", user });
        }
    }
}
