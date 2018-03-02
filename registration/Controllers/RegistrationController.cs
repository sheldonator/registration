using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Validate(string username, string password)
        {
            var result = Zxcvbn.MatchPassword(password, new List<string>{username});
            var count = await PwnedPasswordCheck(password);
            return View("Index", new RegistrationViewModel {PasswordStrengthResult = result, BreachCount = count});
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static async Task<int?> PwnedPasswordCheck(string password)
        {
            var hashedPassword = Hash(password);
            var hash5 = hashedPassword.Substring(0, 5);
            var handler = new HttpClientHandler {UseProxy = false};
            var client = new HttpClient(handler) {BaseAddress = new Uri("https://api.pwnedpasswords.com")};
            var result = await client.GetAsync($"/range/{hash5}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var hashSet = content.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
                var relevantResult = hashSet.SingleOrDefault(h => hash5 + h.Split(":")[0] == hashedPassword);
                return relevantResult == null ? (int?) null : int.Parse(relevantResult.Split(":")[1]);
            }

            return null;
        }

        private static string Hash(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (var b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
