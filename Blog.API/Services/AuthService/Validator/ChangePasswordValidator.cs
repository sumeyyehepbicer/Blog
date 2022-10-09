using Blog.Shared.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Services.AuthService.Validator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequestModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(s => s.Email).NotEmpty().NotNull().WithMessage("Email cannot be empty");
            RuleFor(s => s.Email).EmailAddress().WithMessage("Please enter a valid e-mail address").When(s => !string.IsNullOrEmpty(s.Email));

            RuleFor(s => s.CurrentPassword).NotEmpty().NotNull().WithMessage("Current password cannot be empty")
                .Length(6, 20).WithMessage("Password must be between 6 and 20 characters");
            RuleFor(s => s.NewPassword).NotEmpty().NotNull().WithMessage("New password cannot be empty")
               .Length(6, 20).WithMessage("Password must be between 6 and 20 characters");
            RuleFor(s => s.TryNewPassword).NotEmpty().NotNull().WithMessage("Try new password cannot be empty")
               .Length(6, 20).WithMessage("Password must be between 6 and 20 characters");

            RuleFor(x => x.NewPassword).Equal(x => x.TryNewPassword).WithMessage("Passwords do not match");
        }
    }
}
