using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.MODELS.ApplicationForm;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Interfaces.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace AMS.SERVICES.DataService
{
    public class ApplicationFormService(AMSContext context, ICurrentUser currentUser) : IApplicationFormService
    {
        private readonly AMSContext _context = context;
        private readonly ICurrentUser _currentUser = currentUser;

        public async Task<CreateApplicationFormResponse> CreateApplicationForm(CreateApplicationFormRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var applicant = await GetApplicantUtility(ct);
            var applicationForm = new ApplicationForm
            {
                ApplicantId = applicant.Id,
                ProgramId = request.programId,
                SessionId = 6
            };
            await _context.ApplicationForms.AddAsync(applicationForm,ct);
            await _context.SaveChangesAsync(ct);
            return new CreateApplicationFormResponse(applicationForm.Id);
        }

        public async Task<string> SubmitApplicationForm(SubmitApplicationFormRequest request,CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var applicant = await GetApplicantUtility(ct);
            var applicationForm = await _context.ApplicationForms.FirstOrDefaultAsync(appForm => appForm.ApplicantId == applicant.Id) ?? throw new NotFoundException("Application not found");
            if (applicationForm.IsSubmitted)
                throw new AMSException("Already application exists ");
            var programsApplied = request.ProgramsApplied
                                         .Select((ap,index) => new ProgramApplied
                                         {
                                            DepartmentId = ap.DepartmentId,
                                            TimeShiftId = ap.TimeShiftId,
                                            PreferenceNo = index+1
                                         }).ToList();
            applicant.HeardAboutUniFrom = request.HeardAboutUniFrom;
            applicant.ExpelledFromUni = request.ExpelledFromUni;
            // Always True
            applicationForm.InfoConsent = request.InfoConsent;
            applicationForm.SubmissionDate = DateTime.Now;
            applicationForm.IsSubmitted = true;
            applicationForm.ProgramsApplied = programsApplied;
            await _context.SaveChangesAsync(ct);
            return $"Application submitted with id {applicationForm.Id} successfully";
        }
        //Private Methods
        private async Task<Applicant> GetApplicantUtility(CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId() ?? throw new UnauthorizedException("User is not login");
            var applicant = await _context.Applicants
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync((a) => a.ApplicationUserId == userId, cancellationToken)
                                          .ConfigureAwait(false) ?? throw new NotFoundException("applicant not found");
            return applicant;
        }
    }
}
