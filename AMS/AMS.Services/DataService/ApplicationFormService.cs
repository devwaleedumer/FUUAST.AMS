using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.DOMAIN.Identity;
using AMS.MODELS.ApplicationForms.Ug;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Interfaces.CurrentUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AMS.SERVICES.Identity.Services
{
    public class ApplicationFormService : IApplicationFormService
    {
        private readonly AMSContext _context;
        private readonly IUploadImageService _uploadImageService;
        private readonly ICurrentUser _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationFormService(AMSContext context,
            UserManager<ApplicationUser> userManager,
            ICurrentUser currentUser,
            IUploadImageService uploadImageService)
        {

            _context = context;
            _userManager = userManager;
            _currentUser = currentUser;
            _uploadImageService = uploadImageService;
        }
        public async Task<string> AddApplicationForm(ApplicationRequest request)
        {

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (request == null)
                {
                    throw new ConflictException("request is empty");
                }


                var applicant = new Applicant
                {
                    // ApplicantId= request.Applicant.ApplicantId,
                    ApplicationUserId = 1,//_currentUser.GetUserId()!.Value,
                    FatherName = request.Applicant.FatherName,
                    Cnic = request.Applicant.Cnic,
                    Dob = request.Applicant.Dob,
                    MobileNo = request.Applicant.MobileNo,
                    Gender = request.Applicant.Gender,
                    Religion = request.Applicant.Religion,
                    BloodGroup = request.Applicant.Bloodgroup,
                    Domicile = request.Applicant.Domicile,
                    Province = request.Applicant.Province,
                    PostalCode = request.Applicant.PostalCode,
                    HeardAboutUniFrom = request.Applicant.HeardAboutUniFrom,
                    EmploymentDetails = request.Applicant.EmploymentDetails,
                    PermanentAddress = request.Applicant.PermanentAddress,
                    City = request.Applicant.City,
                    Country = request.Applicant.Country

                };
                _context.Add(applicant);
                await _context.SaveChangesAsync();
                //return the ApplicantId
                var applicantid = await GetApplicantByUserId();
                //request.Applicant.ApplicantId= applicant.ApplicantId;

                await AddGuradianInformation(request, applicant.Id);
                await AddEmergencyContact(request, applicant.Id);
                await AddQualificationDetail(request, applicant.Id);
                await AddApplicationFormAndPrograms(request, applicant.Id);




                await _context.SaveChangesAsync();






                transaction.Commit();


                return "Form successfuly submitted";


            }
            catch (Exception)
            {
                transaction.Rollback();
                return "Form submission failed";
            }

        }

        //Private Methods
        private async Task<Applicant> GetApplicantByUserId()
        {
            var applicant = await _context.Applicants
          .FirstOrDefaultAsync(a => a.ApplicationUserId == _currentUser.GetUserId().GetValueOrDefault());
            return applicant;
        }


        private async Task AddGuradianInformation(ApplicationRequest request, int applicantId)
        {

            if (request == null)
            {
                throw new ConflictException("request is empty");
            }
            var user = new Guardian
            {
                ApplicantId = applicantId,
                Name = request.Guardian.Name,
                Relation = request.Guardian.Relation,
                ContactNo = request.Guardian.ContactNo,
                PermanentAddress = request.Guardian.PermanentAddress


            };
            await _context.AddAsync(user);
            return;
        }
        private async Task AddEmergencyContact(ApplicationRequest request, int applicantId)
        {

            if (request == null)
            {
                throw new ConflictException("request is empty");
            }
            var emergency = new EmergencyContact
            {
                ApplicantId = applicantId,
                Name = request.EmergencyContact.Name,
                Relation = request.EmergencyContact.Relation,
                ContactNo = request.EmergencyContact.ContactNo,
                PermanentAddress = request.EmergencyContact.PermanentAddress
            };

            await _context.AddAsync(emergency);
            return;
        }
        private async Task AddQualificationDetail(ApplicationRequest request, int applicantId)
        {

            if (request == null)
            {
                throw new ConflictException("request is empty");
            }
            foreach (var degree in request.Degree)
            {
                var qualification = new ApplicantDegree
                {
                    ApplicantId = applicantId,
                    BoardOrUniversityName = degree.BoardOrUniversityName,  // Updated to refer to the correct degree property
                    PassingYear = degree.PassingYear,
                    Subject = degree.Subject,
                    RollNo = degree.RollNo,
                    TotalMarks = degree.TotalMarks,
                    ObtainedMarks = degree.ObtainedMarks
                };

                await _context.AddAsync(qualification);
                return;
            }
        }

        private async Task AddApplicationFormAndPrograms(ApplicationRequest request, int applicantId)
        {
            if (request == null)
            {
                throw new ConflictException("request is empty");
            }
            var form = new ApplicationForm
            {
                SessionId = 1,
                ProgramsApplied = request.ProgramApply
                                       .Select((program) => new ProgramApplied
                                       {
                                           DepartmentId = program.DepartmentId,
                                           ProgramId = program.ProgramId,
                                           TimeShiftId = program.TimeShiftId
                                       }).ToList(),
                InfoConsent = request.ApplicationForms.InfoConsent,
                StatusEid = request.ApplicationForms.StatusEid,
                SubmissionDate = request.ApplicationForms.SubmissionDate,
                IsSubmitted = request.ApplicationForms.IsSubmitted,
                HaveValidTest = request.ApplicationForms.HaveValidTest,
                ApplicantId = applicantId,
            };

            await _context.AddAsync(form);
            return;
        }
    }     
}

