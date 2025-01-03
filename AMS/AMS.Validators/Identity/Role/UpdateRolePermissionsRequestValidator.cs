using AMS.MODELS.Identity.Roles;
using AMS.SHARED.Validator;
using FluentValidation;


namespace AMS.VALIDATORS.Identity.Role
{
    public class UpdateRolePermissionsRequestValidator : CustomValidator<UpdateRolePermissionsRequest>
    {
        public UpdateRolePermissionsRequestValidator()
        {
            RuleFor(r => r.Id)
                .NotEmpty();
            RuleFor(r => r.Permissions)
                .NotNull();
        }
    }
}
