using AMS.MODELS.ApplicationForm.Applicant;
using AMS.SHARED.Validator;
using FluentValidation;


namespace AMS.VALIDATORS.ApplicationForms
{
    public class ApplicationRequestValidator : CustomValidator<CreateApplicantPSInfoRequest>
    {
        public ApplicationRequestValidator()
        {
            // Applicant Information validation

            RuleFor(x => x.FatherName)
                .NotEmpty().WithMessage("Father's name is required.");

            RuleFor(x => x.Cnic)
                .NotEmpty().WithMessage("CNIC is required.")
                .Matches(@"\d{13}").WithMessage("CNIC must be 13 digits.");

            RuleFor(x => x.Dob)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.MobileNo)
                .NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"\d{11}").WithMessage("Mobile number must be 11 digits.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.");

            RuleFor(x => x.Religion)
                .NotEmpty().WithMessage("Religion is required.");

            RuleFor(x => x.EmergencyContact)
                .SetValidator(new CreateEmergencyContactRequestValidator());
           
            RuleFor(x => x.Guardian)
                .SetValidator(new CreateGuardianRequestValidator());

                
        }
    }
}





