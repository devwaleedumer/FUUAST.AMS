using AMS.MODELS.ApplicationForms.Ug;
using AMS.SHARED.Validator;
using FluentValidation;


namespace AMS.VALIDATORS.ApplicationForms.Ug
{
    public class ApplicationRequestValidator  : CustomValidator<ApplicationRequest>
    {     
            public ApplicationRequestValidator()
            {
                // Applicant Information validation
                RuleFor(x => x.Applicant).NotNull().WithMessage("Applicant information is required.");

                RuleFor(x => x.Applicant.FatherName)
                    .NotEmpty().WithMessage("Father's name is required.");

                RuleFor(x => x.Applicant.Cnic)
                    .NotEmpty().WithMessage("CNIC is required.")
                    .Matches(@"\d{13}").WithMessage("CNIC must be 13 digits.");

                RuleFor(x => x.Applicant.Dob)
                    .NotEmpty().WithMessage("Date of birth is required.")
                    .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");

                RuleFor(x => x.Applicant.MobileNo)
                    .NotEmpty().WithMessage("Mobile number is required.")
                    .Matches(@"\d{11}").WithMessage("Mobile number must be 11 digits.");

                RuleFor(x => x.Applicant.Gender)
                    .NotEmpty().WithMessage("Gender is required.");

                RuleFor(x => x.Applicant.Religion)
                    .NotEmpty().WithMessage("Religion is required.");

                        //Guardian Validation
                RuleFor(x => x.Guardian)
                    .NotNull().WithMessage("Guardian information is required.");

                RuleFor(x => x.Guardian.Name)
                    .NotEmpty().WithMessage("Guardian's name is required.");

                RuleFor(x => x.Guardian.Relation)
                    .NotEmpty().WithMessage("Guardian's relation is required.");

                RuleFor(x => x.Guardian.ContactNo)
                    .NotEmpty().WithMessage("Guardian's contact number is required.")
                    .Matches(@"\d{11}").WithMessage("Contact number must be 11 digits.");

                // Emergency Contact Information validation
                RuleFor(x => x.EmergencyContact)
                    .NotNull().WithMessage("Emergency contact information is required.");

                RuleFor(x => x.EmergencyContact.Name)
                    .NotEmpty().WithMessage("Emergency contact name is required.");

                RuleFor(x => x.EmergencyContact.Relation)
                    .NotEmpty().WithMessage("Emergency contact relation is required.");

                RuleFor(x => x.EmergencyContact.ContactNo)
                    .NotEmpty().WithMessage("Emergency contact number is required.")
                    .Matches(@"\d{11}").WithMessage("Contact number must be 11 digits.");

            /// Applicant Degree Validator 
            RuleForEach(x => x.Degree).ChildRules(degree =>
            {
                degree.RuleFor(d => d.BoardOrUniversityName)
                    .NotEmpty().WithMessage("Board or University name is required.");

                degree.RuleFor(d => d.PassingYear)
                    .NotEmpty().WithMessage("Passing year is required.")
                    .Must(x => x <= DateTime.Now.Year).WithMessage("Passing year must be in the past or current year.");

                degree.RuleFor(d => d.Subject)
                    .NotEmpty().WithMessage("Subject is required.");

                degree.RuleFor(d => d.RollNo)
                    .NotEmpty().WithMessage("Roll number is required.");

                degree.RuleFor(d => d.TotalMarks)
                    .GreaterThan(0).WithMessage("Total marks must be greater than zero.");

                degree.RuleFor(d => d.ObtainedMarks)
                    .GreaterThanOrEqualTo(0).WithMessage("Obtained marks cannot be negative.")
                    .LessThanOrEqualTo(d => d.TotalMarks).WithMessage("Obtained marks cannot exceed total marks.");
            });

            //Program Applied Validator 
            RuleForEach(x => x.ProgramApply).ChildRules(program =>
            {

                program.RuleFor(p => p.TimeShiftId)
                    .NotNull().WithMessage("TimeShiftId is required");

                program.RuleFor(p => p.DepartmentId)
                    .NotNull().WithMessage("DepartmentId is required");

                program.RuleFor(p => p.ProgramId)
                    .NotNull().WithMessage("ProgramId is required");
            });

            // Validate ApplicationForms properties
            RuleFor(x => x.ApplicationForms)
                .NotNull().WithMessage("ApplicationForms is required");

            RuleFor(x => x.ApplicationForms.InfoConsent)
                .NotNull().WithMessage("InfoConsent is required");

            RuleFor(x => x.ApplicationForms.StatusEid)
                .NotEmpty().WithMessage("StatusEid is required");


            RuleFor(x => x.ApplicationForms.HaveValidTest)
                .NotEmpty().WithMessage("HaveValidTest is required");

        }
    }
    }
        

    
    

