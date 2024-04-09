using AMS.MODELS.Identity.User;
using AMS.SHARED.Validator;
using FluentValidation;

namespace AMS.VALIDATORS.Identity.User
{
    public class ChangePasswordRequestValidator : CustomValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(p => p.Password)
                .NotEmpty();

            RuleFor(p => p.NewPassword)
                .NotEmpty();

            RuleFor(p => p.ConfirmNewPassword)
                .Equal(p => p.NewPassword)
                    .WithMessage("Passwords do not match.");
        }
    }
}
