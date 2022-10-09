using Blog.Shared.Models.RequestModels;
using FluentValidation;

namespace Blog.API.Services.AuthService.Validator
{
    public class AuthValidator : AbstractValidator<AuthRequestModel>
    {
        public AuthValidator()
        {
            RuleFor(s => s.Email).NotEmpty().NotNull().WithMessage("Email cannot be empty");
            RuleFor(s => s.Email).EmailAddress().WithMessage("Please enter a valid e-mail address").When(s=>!string.IsNullOrEmpty(s.Email));
            RuleFor(s => s.Password).NotEmpty().NotNull().WithMessage("Password cannot be empty")
                .Length(6,20).WithMessage("Password must be between 6 and 20 characters");


        }
    }
}
