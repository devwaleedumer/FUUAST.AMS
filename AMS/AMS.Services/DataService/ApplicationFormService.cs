using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.MODELS.ApplicationForm;
using AMS.MODELS.Dashboard;
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
            await _context.ApplicationForms.AddAsync(applicationForm, ct);
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
                                         .Select((ap, index) => new ProgramApplied
                                         {
                                             DepartmentId = ap.DepartmentId,
                                             TimeShiftId = ap.TimeShiftId,
                                             PreferenceNo = index + 1
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
        public async Task<string> EditSubmittedApplication(EditSubmitApplicationFormRequest request, CancellationToken ct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                ArgumentNullException.ThrowIfNull(request, nameof(request));
                var applicant = await GetApplicantUtility(ct);
                applicant.ExpelledFromUni = request.ExpelledFromUni;
                applicant.HeardAboutUniFrom = request.HeardAboutUniFrom;
                var programsApplied = await _context.ProgramsApplied
                                                    .Where(pa => pa.ApplicationFormId == request.Id)
                                                    .ToListAsync(ct);
                programsApplied.ForEach((entity) =>
                {
                    var result = request.ProgramsApplied.FirstOrDefault((req) => req.Id == entity.Id);
                    if (result is not null)
                    {
                        entity.DepartmentId = result.DepartmentId;
                        entity.TimeShiftId = result.TimeShiftId;
                    }
                });
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return $"Application form updated successfully {request.Id}";
            }
            catch 
            {
                await transaction.RollbackAsync(ct);
                throw new AMSException("Application form cann't be updated");
            }
        }
        public async Task<SubmitApplicationResponse> GetSubmittedApplication(CancellationToken ct)
        {
            var applicant = await GetApplicantUtility(ct);
            var response = await _context.ApplicationForms
                                                .AsNoTracking()
                                                .Include(x  => x.ProgramsApplied!)
                                                .ThenInclude(y => y.Department)
                                                .Where(x => x.ApplicantId == applicant.Id && x.IsSubmitted == true)
                                                .AsSplitQuery()
                                                .Select((application)
                                                   => new SubmitApplicationResponse
                                                   {
                                                       Id = application.Id,
                                                       ProgramId = application.ProgramId ?? 0,
                                                       InfoConsent = application.InfoConsent ?? false,
                                                       ProgramsApplied = application.ProgramsApplied!.Select((programApplied) => new SubmitApplicationAppliedPrograms(programApplied.Id,programApplied.DepartmentId, programApplied.TimeShiftId,programApplied.Department!.FaculityId)).ToList(),
                                                   })
                                                .FirstOrDefaultAsync(ct)
                                                .ConfigureAwait(false) ?? throw new NotFoundException("No application found");

            
            var timeShiftsList = new List<List<TimeShiftOptions>>();
            var departmentsList = new List<List<DepartmentOptions>>();

            foreach (var pa in response.ProgramsApplied)
            {
                var timeShiftsResult = await _context.ProgramDepartments
                                                    .AsNoTracking()
                                                    .Where(pd => pd.DepartmentId == pa.DepartmentId && pd.ProgramId == response.ProgramId)
                                                    .Select((programDepartment) => programDepartment.TimeShift)
                                                    .ToListAsync();
                timeShiftsList.Add(timeShiftsResult.Select(x => new TimeShiftOptions(x!.Id,x.Name)).ToList());

                var departmentsResult = await _context.Faculties
                                            .AsNoTracking()
                                            .Include(d => d.Departments)
                                            .Where(faculty => faculty.Id == pa.FacultyId)
                                            .Select((faculty) => faculty.Departments)
                                            .FirstOrDefaultAsync(ct);
                departmentsList.Add(departmentsResult!.Select((x) => new DepartmentOptions(x.Id,x.Name)).ToList());

            };
            response.Departments = departmentsList;
            response.Shifts = timeShiftsList;
            response.ExpelledFromUni = applicant.ExpelledFromUni ?? string.Empty;
            response.HeardAboutUniFrom = applicant.HeardAboutUniFrom ?? string.Empty;
            return response;

        }
        public  Task<ApplicantDashboardResponse> GetApplicationFormStatus(int userId, CancellationToken ct)
            =>  GetStatusListAsync(userId, ct);

        //Private Methods
        private async Task<Applicant> GetApplicantUtility(CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId() ?? throw new UnauthorizedException("User is not login");
            var applicant = await _context.Applicants
                                          .FirstOrDefaultAsync((a) => a.ApplicationUserId == userId, cancellationToken)
                                          .ConfigureAwait(false) ?? throw new NotFoundException("applicant not found");
            return applicant;
        }
        private async Task<ApplicantDashboardResponse> GetStatusListAsync(int userId, CancellationToken ct)
        {
            var response = new ApplicantDashboardResponse();
            var applicant = await _context.Applicants.FirstOrDefaultAsync(x => x.ApplicationUserId == userId, ct);
            if (applicant == null)
            {
                response.FormStatuses = new List<FormStatus>
                {
                    new FormStatus("Personal Information","In Progress"),
                    new FormStatus("Program Type Selection","Not Started"),
                    new FormStatus("Academic Records","Not Started"),
                    new FormStatus("Programs Selection & Form Submission","Not Started"),
                };
                response.CompletedSteps = 0;
                response.LastModified = null;
                return response;
            }
            var applicationForm = await _context.ApplicationForms
                                                      .AsNoTracking()
                                                      .FirstOrDefaultAsync(x => x.ApplicantId == applicant.Id);
            if (applicationForm is null)
            {
                response.FormStatuses = new List<FormStatus>
                {
                    new FormStatus("Personal Information","Completed"),
                    new FormStatus("Program Type Selection","In Progress"),
                    new FormStatus("Academic Records","Not Started"),
                    new FormStatus("Programs Selection & Form Submission","Not Started"),
                };
                response.CompletedSteps = 1;
                response.LastModified = applicant.UpdatedDate;
                return response;
            }
            var degreesExist = await _context.ApplicantDegrees.AnyAsync((x) => x.ApplicantId == applicant.Id, ct);
            if (!degreesExist)
            {
                response.FormStatuses = new List<FormStatus>
                {
                    new FormStatus("Personal Information","Completed"),
                    new FormStatus("Program Type Selection","Completed"),
                    new FormStatus("Academic Records","In Progress"),
                    new FormStatus("Programs Selection & Form Submission","Not Started"),
                };
                response.CompletedSteps = 2;
                response.LastModified = applicant.UpdatedDate;
                return response;
            }
            if (!applicationForm.IsSubmitted)
            {
                response.FormStatuses = new List<FormStatus>
                {
                    new FormStatus("Personal Information","Completed"),
                    new FormStatus("Program Type Selection","Completed"),
                    new FormStatus("Academic Records","Completed"),
                    new FormStatus("Programs Selection & Form Submission","In Progress"),
                };
                response.CompletedSteps = 3;
                response.LastModified = applicant.InsertedDate;
                return response;
            }
            response.FormStatuses = new List<FormStatus>
                {
                    new FormStatus("Personal Information","Completed"),
                    new FormStatus("Program Type Selection","Completed"),
                    new FormStatus("Academic Records","Completed"),
                    new FormStatus("Programs Selection & Form Submission","Completed"),
                };
            response.CompletedSteps = 4;
            response.LastModified = applicant.UpdatedDate;
            return response;
        }
    }
}