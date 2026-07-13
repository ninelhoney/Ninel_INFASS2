using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ninel_INFASS2.Models;

namespace Ninel_INFASS2.Controllers
{
    public class HomeController : Controller
    {
        public class AccountController : Controller
        {
            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Register(RegisterModel model)
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Login");
                }

                return View(model);
            }
        }
    }
}
