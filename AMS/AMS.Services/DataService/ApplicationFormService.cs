using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.Interfaces.Mail;
using AMS.MODELS.ApplicationForm;
using AMS.MODELS.Dashboard;
using AMS.MODELS.Models.Mail;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Enums.AMS;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Interfaces.CurrentUser;
using AMS.SHARED.Interfaces.Hangfire;
using Microsoft.EntityFrameworkCore;

namespace AMS.SERVICES.DataService
{
    public class ApplicationFormService(AMSContext context, ICurrentUser currentUser,IEmailTemplateService emailTemplateService,IJobService job,IMailService mailService) : IApplicationFormService
    {
        private readonly AMSContext _context = context;
        private readonly ICurrentUser _currentUser = currentUser;
        private readonly IJobService _job = job;
        private readonly IMailService _mailService = mailService;
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;



       
        public async Task<CreateApplicationFormResponse> CreateApplicationForm(CreateApplicationFormRequest request, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var applicant = await GetApplicantUtility(ct);
            var sessionId = await getsession();
            var applicationForm = new ApplicationForm
            {
                ApplicantId = applicant.Id,
                ProgramId = request.programId,
                SessionId = sessionId,
            };
            await _context.ApplicationForms.AddAsync(applicationForm, ct);
            await _context.SaveChangesAsync(ct);
            return new CreateApplicationFormResponse(applicationForm.Id);
        }
        public async Task<string> AddApplicationFormPrograms(SubmitApplicationFormRequest request,CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            var applicant = await GetApplicantUtility(ct);
            var applicationForm = await _context.ApplicationForms.FirstOrDefaultAsync(appForm => appForm.ApplicantId == applicant.Id,ct) ?? throw new NotFoundException("Application not found");
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
            applicationForm.VerificationStatusEid = 1;
            // Always True
            applicationForm.InfoConsent = request.InfoConsent;
            applicationForm.ProgramsApplied = programsApplied;
            var feeChallan = new FeeChallan
            {
                NoOfProgramsApplied = programsApplied.Count,
                DueTill = DateTime.Now.AddDays(15),
                TotalFee = programsApplied.Count * 2000,
                ApplicationFormId = applicationForm.Id,
                IssuedOn = DateTime.Now,
            };
            _context.FeeChallans.Add(feeChallan);
            await _context.SaveChangesAsync(ct);

            // var email = _currentUser.GetUserEmail();
            // var userName = _currentUser.Name;
            // var emailModel = new ConfirmApplicationEmail(userName!, applicationForm.Id,applicationForm.SubmissionDate.Value);
            // var mailRequest = new MailRequest(
            //    // To user mail
            //    new List<string> { email! },
            // //subject
            //    "Application Confirmation",
            // //body
            //    _emailTemplateService.GenerateEmailTemplate("admission-confirmation", emailModel));
            // // fire and forget pattern so that's why we are sending cancellation.none token  
            // _job.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
            return "Application submitted with successfully";
        }
        public async Task<string> EditApplicationFormPrograms(EditSubmitApplicationFormRequest request, CancellationToken ct)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(ct);
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
                var feeChallan = await _context.FeeChallans
                                         .FirstOrDefaultAsync(x => x.ApplicationFormId == request.Id);
                if (feeChallan!.NoOfProgramsApplied == request.ProgramsApplied.Count)
                {
                    feeChallan.NoOfProgramsApplied = request.ProgramsApplied.Count;
                    feeChallan.TotalFee = feeChallan.NoOfProgramsApplied * 2000;
                    feeChallan.UpdatedDate = DateTime.Now;
                }
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return $"Application form updated successfully {request.Id}";
            }
            catch 
            {
                await transaction.RollbackAsync(ct);
                throw new AMSException("Application form can't be updated");
            }
        }
        public async Task<SubmitApplicationResponse> GetApplicationFormPrograms(CancellationToken ct)
        {
            var applicant = await GetApplicantUtility(ct);
            var response = await _context.ApplicationForms
                                                .AsNoTracking()
                                                .Include(x  => x.ProgramsApplied!)
                                                .ThenInclude(y => y.Department)
                                                .Where(x => x.ApplicantId == applicant.Id)
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
                                                    .ToListAsync(ct);
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

        public async Task<ApplicationDetailResponse> ApplicationDetailsByApplicantId(int applicantId,CancellationToken ct)
        {
            var result = await _context.ApplicationForms
                .AsNoTracking()
                .Include(x => x.Applicant)
                .Include(x => x.FeeChallan)
                .Include(x => x.Program)
                .Where(application => applicantId == application.ApplicantId && application.VerificationStatusEid != null )
                .Select(ap => new ApplicationDetailResponse
                {
                  FormNo = ap.Id,
                  FullName = ap.Applicant!.FullName,
                  Cnic = ap.Applicant!.Cnic,
                  FeeChallanNo = ap.FeeChallan!.Id,
                  TotalFee = ap.FeeChallan!.TotalFee,
                  Program = ap.Program!.Name,
                  VerificationStatus = ap.VerificationStatusEid.ToString()!
                })
                .FirstOrDefaultAsync(ct).ConfigureAwait(false);
            if (result is null)
            {
                throw new NotFoundException("No application found");
            }
            result.VerificationStatus = Enum.GetName(typeof(VerificationStatus), int.Parse(result.VerificationStatus));
            var challanStatus = await _context.FeeChallanSubmissionDetails.AnyAsync(x => x.FeeChallanId == result.FeeChallanNo);
            result.ChallanStatus = challanStatus ? "Paid" : "Unpaid" ;
            result.NoOfProgramsApplied = await _context.ProgramsApplied.CountAsync(x => x.ApplicationFormId == result.FormNo);
            return result;
        }

        //Private Methods
        private async Task<Applicant> GetApplicantUtility(CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId() ?? throw new UnauthorizedException("Authentication Failed.");
            var applicant = await _context.Applicants
                                          .FirstOrDefaultAsync((a) => a.ApplicationUserId == userId, cancellationToken)
                                          .ConfigureAwait(false) ?? throw new NotFoundException("applicant not found");
            return applicant;
        }
        private async Task<ApplicantDashboardResponse> GetStatusListAsync(int userId, CancellationToken ct)
        {
            var response = new ApplicantDashboardResponse();
            var applicant = await _context.Applicants.FirstOrDefaultAsync(x => x.ApplicationUserId == userId, ct);
            if (applicant != null && applicant.FatherName == null)
            {
                response.FormStatuses =
                [
                    new FormStatus("Personal Information", "In Progress"),
                    new FormStatus("Program Type Selection", "Not Started"),
                    new FormStatus("Academic Records & Programs Selection", "Not Started"),
                    new FormStatus("Fee Submission", "Not Started")
                ];
                response.CompletedSteps = 0;
                response.LastModified = null;
                return response;
            }
          
            var applicationForm = await _context.ApplicationForms
                                                      .AsNoTracking()
                                                      .FirstOrDefaultAsync(x => x.ApplicantId == applicant.Id,ct);
            if (applicationForm is null)
            {
                response.FormStatuses =
                [
                    new FormStatus("Personal Information", "Completed"),
                    new FormStatus("Program Type Selection", "In Progress"),
                    new FormStatus("Academic Records & Programs Selection", "Not Started"),
                    new FormStatus("Fee Submission", "Not Started")
                ];
                response.CompletedSteps = 1;
                response.LastModified = applicant.UpdatedDate;
                return response;
            }
            var degreesExist = await _context.ApplicantDegrees.AnyAsync((x) => x.ApplicantId == applicant.Id, ct);
            if (!degreesExist)
            {
                response.FormStatuses =
                [
                    new FormStatus("Personal Information", "Completed"),
                    new FormStatus("Program Type Selection", "Completed"),
                    new FormStatus("Academic Records & Programs Selection", "In Progress"),
                    new FormStatus("Fee Submission", "Not Started")
                ];
                response.CompletedSteps = 2;
                response.LastModified = applicant.UpdatedDate;
                return response;
            }
            if (!applicationForm.IsSubmitted)
            {
                response.FormStatuses =
                [
                    new FormStatus("Personal Information", "Completed"),
                    new FormStatus("Program Type Selection", "Completed"),
                    new FormStatus("Academic Records & Programs Selection", "Completed"),
                    new FormStatus("Fee Submission", "In Progress")
                ];
                response.CompletedSteps = 3;
                response.LastModified = applicant.InsertedDate;
                return response;
            }
            response.FormStatuses =
            [
                new FormStatus("Personal Information", "Completed"),
                new FormStatus("Program Type Selection", "Completed"),
                new FormStatus("Academic Records & Programs Selection", "Completed"),
                new FormStatus("Fee Submission", "Completed")
            ];
            response.CompletedSteps = 4;
            response.LastModified = applicant.UpdatedDate;
            return response;
        }
        private async Task<int> getsession()
        {
            return (await _context.Sessions.OrderBy(x =>x.Id).LastAsync()).Id;
    }
    }
   
}