using Blog.Shared.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.AuthService.Validator
{
    public class RegisterValidator : AbstractValidator<RegisterRequestModel>
    {
        public RegisterValidator()
        {

            RuleFor(s => s.FirstName).NotEmpty().NotNull().WithMessage("FirstName cannot be empty");           
            RuleFor(s => s.LastName).NotEmpty().NotNull().WithMessage("LastName cannot be empty");      
            
            RuleFor(s => s.PhoneNumber).NotEmpty().NotNull().WithMessage("PhoneNumber cannot be empty");
            RuleFor(s => s.PhoneNumber).NotEmpty().NotNull().WithMessage("Password cannot be empty")
                .Length(10).WithMessage("PhoneNumber must be 10 characters")
                .Must(BeAValidPhoneNumber).WithMessage("Phone number cannot start with 0");
            
            RuleFor(s => s.Email).NotEmpty().NotNull().WithMessage("Email cannot be empty");
            RuleFor(s => s.Email).EmailAddress().WithMessage("Please enter a valid e-mail address").When(s => !string.IsNullOrEmpty(s.Email));
            RuleFor(s => s.Password).NotEmpty().NotNull().WithMessage("Password cannot be empty")
                .Length(6, 20).WithMessage("Password must be between 6 and 20 characters");
                       
        }
        private bool BeAValidPhoneNumber(string phoneNumber)
        {
            var startZero = phoneNumber.Substring(0, 1);
            if (startZero=="0")
            {
                return false;
            }
            return true;
        }
    }
}
