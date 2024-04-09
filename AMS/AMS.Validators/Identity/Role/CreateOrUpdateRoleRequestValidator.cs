

using AMS.MODELS.Identity.Roles;
using AMS.SERVICES.Identity.Interfaces;
using AMS.SHARED.Validator;
using FluentValidation;

namespace AMS.VALIDATORS.Identity.Role
{
    public class CreateOrUpdateRoleRequestValidator : CustomValidator<CreateOrUpdateRoleRequest>
    {
        public CreateOrUpdateRoleRequestValidator(IRoleService roleService) =>
            RuleFor(r => r.Name)
                .NotEmpty()
                .MustAsync(async (role, name, _) => !await roleService.ExistsAsync(name, role.Id))
                    .WithMessage("Similar Role already exists.");
    }
}
