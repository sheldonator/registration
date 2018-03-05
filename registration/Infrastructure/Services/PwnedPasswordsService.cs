using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;

namespace registration.Infrastructure.Services
{
    public class PwnedPasswordsService
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger = Log.Logger;

        public PwnedPasswordsService()
        {
            var handler = new HttpClientHandler { UseProxy = false };
            _client = new HttpClient(handler) { BaseAddress = new Uri("https://api.pwnedpasswords.com") };
        }

        public async Task<int?> PwnedPasswordCheck(string hashedPassword)
        {
            var hash5 = hashedPassword.Substring(0, 5);
            var result = await _client.GetAsync($"/range/{hash5}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var hashSet = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var relevantResult = hashSet.SingleOrDefault(h => hash5 + h.Split(":")[0] == hashedPassword);
                return relevantResult == null ? 0 : int.Parse(relevantResult.Split(":")[1]);
            }

            return null;
        }
    }
}
