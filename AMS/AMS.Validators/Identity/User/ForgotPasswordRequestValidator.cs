using AMS.MODELS.Identity.User;
using AMS.SHARED.Validator;
using FluentValidation;


namespace AMS.VALIDATORS.Identity.User
{
    public class ForgotPasswordRequestValidator : CustomValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator() =>
            RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                    .WithMessage("Invalid Email Address.");
    }
}
