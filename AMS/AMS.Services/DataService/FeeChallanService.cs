using AMS.DATA;
using AMS.DOMAIN.Entities.AMS;
using AMS.Interfaces.Mail;
using AMS.MODELS.ApplicationForm;
using AMS.MODELS.ApplicationForm.Applicant;
using AMS.MODELS.FeeChallan;
using AMS.MODELS.Models.Mail;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Exceptions;
using AMS.SHARED.Interfaces.CurrentUser;
using AMS.SHARED.Interfaces.Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.DataService
{
    public class FeeChallanService(AMSContext context, ILocalFileStorageService localStorage, ICurrentUser currentUser,IMailService mailService, IJobService job, IEmailTemplateService emailTemplateService) : IFeeChallanService
    {
        private readonly IJobService _job = job;
        private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;
        private readonly AMSContext _context = context;
        private readonly IMailService _mailService = mailService;
        private readonly ILocalFileStorageService _localStorage = localStorage;
        private readonly ICurrentUser _currentUser = currentUser;
        public async Task<FeeChallanReportDto> GetFeeChallanData(int applicantId, CancellationToken ct)
        {
            var result = await _context.ApplicationForms
                .Include(x => x.Applicant)
                .Include(x => x.Program)
                .Include(x => x.Session)
                .Include(x => x.FeeChallan)
                .Where(x => x.ApplicantId == applicantId)
                .Select(x => new FeeChallanReportDto
                {
                    ApplicationFormNo = $"Form-{x.Session!.Name}-{x.Session.StartDate!.Value.Year}-{x.Id}",
                    CNIC = x.Applicant!.Cnic,
                    FullName = x.Applicant.FullName,
                    FatherName = x.Applicant.FatherName ?? "",
                    AdmissionSession = $"{x.Session!.Name} {x.Session.StartDate!.Value.Year}",
                    Program = x.Program!.Name,
                    NoOfProgramsApplied = x.FeeChallan!.NoOfProgramsApplied,
                    VoucherNo = x.FeeChallan.Id,
                    AmountInWords = FormatIntoWords(x.FeeChallan.NoOfProgramsApplied * 2000)
                })
                .FirstOrDefaultAsync(ct);
            if (result is null)
            {
                throw new NotFoundException($"Application form does 'nt exists for {applicantId}");
            }
            return result;
        }

        public async Task<bool> FeeChallanExists(int applicantId,CancellationToken ct)
        {
            var applicationForm = await _context.ApplicationForms
                .Include(a => a.FeeChallan)
                .Where(x => x.ApplicantId == applicantId).FirstOrDefaultAsync(ct);

            if (applicationForm is  null)
                return false;

            if (applicationForm.FeeChallan is null)
                return false ;
            return true;
        }

        public async Task UploadFeeChallanImage(int feeChallanId, FeeChallanSubmissionRequest request, CancellationToken ct)
        {
           var image =  await _localStorage.UploadAsync<FeeChallan>(request.ImageRequest, SHARED.Enums.Shared.FileType.Image, ct);
           var feeChallan  = await _context.FeeChallans.Where(x => x.Id == feeChallanId)
                                                 .Include(x => x.FeeChallanSubmissionDetail)
                                                 .FirstOrDefaultAsync(ct);
            if (feeChallan is null )
            {
                throw new NotFoundException($"Fee Challan not found ${feeChallanId}");
            }

            if (feeChallan.FeeChallanSubmissionDetail is null)
            {
                var feeChallanSubmissionEntity = new FeeChallanSubmissionDetail
                {
                    DocumentUrl = image,
                    FeeChallanId = feeChallanId,
                    SubmissionDate = DateTime.Now,
                };
                // Add
                feeChallan.FeeChallanSubmissionDetail = feeChallanSubmissionEntity;
                var applicationForm = await _context.ApplicationForms.FindAsync(feeChallan.ApplicationFormId);
                applicationForm!.IsSubmitted = true;
                applicationForm!.SubmissionDate = DateTime.Now; 
                await _context.SaveChangesAsync(ct);
                var email = _currentUser.GetUserEmail();
                var userName = _currentUser.Name;
                var emailModel = new ConfirmApplicationEmail(userName!, applicationForm.Id, applicationForm.SubmissionDate.Value);
                var mailRequest = new MailRequest(
                   // To user mail
                   new List<string> { email! },
                   //subject
                   "Application Confirmation",
                   //body
                   _emailTemplateService.GenerateEmailTemplate("admission-confirmation", emailModel));
                // fire and forget pattern so that's why we are sending cancellation.none token  
                _job.Enqueue(() => _mailService.SendAsync(mailRequest, CancellationToken.None));
                return;
            }
            //update
            _localStorage.Remove(feeChallan.FeeChallanSubmissionDetail.DocumentUrl);
            feeChallan.FeeChallanSubmissionDetail.DocumentUrl = image;  
            feeChallan.FeeChallanSubmissionDetail.SubmissionDate = DateTime.Now;
            await _context.SaveChangesAsync(ct);
        }

        private static string FormatIntoWords(int amount)
        {
            if (amount == 2000)
            {
                return "Two Thousands";
            }

            if (amount == 4000)
            {
                return "Four Thousands";
            }
            if (amount == 6000)
            {
                return "Six Thousands";
            }
            if (amount == 8000)
            {
                return "Eight Thousands";
            }

            return "Ten Thousands";
        }
    }
}