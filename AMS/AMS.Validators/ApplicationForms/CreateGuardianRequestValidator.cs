using AMS.MODELS.ApplicationForm.Applicant;
using AMS.SHARED.Validator;
using FluentValidation;

namespace AMS.VALIDATORS.ApplicationForms
{
    public class CreateGuardianRequestValidator : CustomValidator<CreateGuardianRequest>
    {
        public CreateGuardianRequestValidator()
        {
            //Guardian Validation
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Guardian's name is required.");

            RuleFor(x => x.Relation)
                    .NotEmpty().WithMessage("Guardian's relation is required.");

            RuleFor(x => x.ContactNo)
                    .NotEmpty().WithMessage("Guardian's contact number is required.")
                    .Matches(@"\d{11}").WithMessage("Contact number must be 11 digits.");
            RuleFor(x => x.PermanentAddress)
                  .NotNull().WithMessage("Guardian's permanent address is required.");

        }
    }
}
