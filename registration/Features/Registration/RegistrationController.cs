using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using registration.Features.Registration.Models;
using registration.Infrastructure.Extensions;
using registration.Infrastructure.Services;

namespace registration.Features.Registration
{
    public class RegistrationController : Controller
    {
        private readonly PwnedPasswordsService _ppService;

        public RegistrationController(PwnedPasswordsService ppService)
        {
            _ppService = ppService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Validate(RegistrationForm request)
        {
            var result = Zxcvbn.Zxcvbn.MatchPassword(request.Password, new List<string>{ request.Email });
            var count = await _ppService.PwnedPasswordCheck(request.Password.Sha1Hash());
            return View("Index", new RegistrationViewModel {PasswordStrengthResult = result, BreachCount = count});
        }
    }
}
