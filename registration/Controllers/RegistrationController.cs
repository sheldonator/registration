using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using registration.Models;

namespace registration.Controllers
{
    using Zxcvbn;

    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Validate(string username, string password)
        {
            var result = Zxcvbn.MatchPassword(password, new List<string>{username});
            return View("Index", new RegistrationViewModel {PasswordStrengthResult = result});
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
