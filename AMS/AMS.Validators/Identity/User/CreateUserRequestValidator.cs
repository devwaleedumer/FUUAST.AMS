using AMS.MODELS.Identity.User;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SERVICES.Identity.Services;
using AMS.SHARED.Validator;
using FluentValidation;



namespace AMS.VALIDATORS.Identity.User
{
    public class CreateUserRequestValidator : CustomValidator<CreateUserRequest>
    {
        private readonly IUserService _userService;
        public CreateUserRequestValidator(IUserService userService)
        {
            _userService = userService;
            RuleFor(u => u.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                    .WithMessage("Invalid Email Address.");
            RuleFor(p => p.FullName).Cascade(CascadeMode.Stop)
                .NotEmpty();

            RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(p => p.ConfirmPassword).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Equal(p => p.Password);
        }
    }
}
