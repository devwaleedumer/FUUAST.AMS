using AMS.MODELS.Faculity;
using AMS.SHARED.Validator;
using FluentValidation;

namespace AMS.VALIDATORS.Faculity;

public class CreateFacultyValidator :CustomValidator<CreateFacultyRequest>
{
    public CreateFacultyValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty();
    }
}