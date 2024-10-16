using AMS.MODELS.ApplicationForm.Applicant;
using AMS.SHARED.Validator;
using FluentValidation;


namespace AMS.VALIDATORS.ApplicationForms
{
    public class CreateEmergencyContactRequestValidator : CustomValidator<CreateEmergencyContactRequest>
    {
        public CreateEmergencyContactRequestValidator()
        {
            // Emergency Contact Information validation
            RuleFor(x => x.PermanentAddress)
                .NotEmpty().WithMessage("Emergency contact information is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Emergency contact name is required.");

            RuleFor(x => x.Relation)
                .NotEmpty().WithMessage("Emergency contact relation is required.");

            RuleFor(x => x.ContactNo)
                .NotEmpty().WithMessage("Emergency contact number is required.")
                .Matches(@"\d{11}").WithMessage("Contact number must be 11 digits.");
        }
    }
}
