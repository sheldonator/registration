﻿using Zxcvbn;

namespace registration.Models
{
    public class RegistrationViewModel
    {
        public Result PasswordStrengthResult { get; set; }
        public int? BreachCount { get; set; }
    }
}
