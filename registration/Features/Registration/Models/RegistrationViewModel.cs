using Zxcvbn;

namespace registration.Features.Registration.Models
{
    public class RegistrationViewModel
    {
        public Result PasswordStrengthResult { get; set; }
        public int? BreachCount { get; set; }
    }
}
