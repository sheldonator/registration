using FluentValidation;

namespace registration.Features.Registration.Models
{
    public class RegistrationForm
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegistrationFormValidator : AbstractValidator<RegistrationForm>
    {
        public RegistrationFormValidator()
        {
            RuleFor(r => r.Email).NotNull();
            RuleFor(r => r.Password).NotNull();
        }
    }
}
