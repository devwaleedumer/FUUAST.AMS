using AMS.DATA;
using AMS.MODELS.Academicyear;
using AMS.MODELS.ApplicantManagement;
using AMS.MODELS.ApplicationForm.Applicant;
using AMS.SERVICES.Dapper;
using AMS.SERVICES.IDataService;
using AMS.SHARED.Enums.AMS;
using AMS.SHARED.Exceptions;
using Dapper;
using Mapster;
using Mapster.Utils;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.DataService
{
    public class ApplicantManagementService(AMSContext context, IDapperService dapperService) : IApplicantManagementService
    {
        private readonly AMSContext _context = context;
        private readonly IDapperService _dapperService = dapperService;


        public async Task <List<ApplicantInfoList>> GetAllApplicantDetails(ApplicantInfoRequest user)
        {
            DynamicParameters parameters = new();
           
            parameters.Add("@UserName",user.UserName);
           
            parameters.Add("@EMAIL", user.Email);
            parameters.Add("@VerificationStatusEid", user.VerificationStatusEid);
            ////    parameters.Add("@PageNumber", user.PageNumber);
          //  parameters.Add("@PageSize", user.PageSize);


            var userList = await _dapperService.ReturnListAsync<ApplicantInfoList>("GetApplicantDetails", parameters);
            if (userList == null || !userList.Any())
            {
                throw new NotFoundException("Applicant data not found");
            }
            
            // Deserialize the JSON into the DegreeDetails list
            foreach (var applicant in userList)
            {
                if (!string.IsNullOrEmpty(applicant.DegreeDetails))
                {
                    // Deserialize the JSON into the DegreeDetails list
                    var degrees = JsonConvert.DeserializeObject<List<DegreeInfoList>>(applicant.DegreeDetails);
                    applicant.Degrees = degrees ?? new List<DegreeInfoList>();
                }
                else
                {
                    applicant.Degrees = new List<DegreeInfoList>();
                }
            }
            return userList.Adapt<List<ApplicantInfoList>>();
        }
            

        

        public async Task<List<Applicantmanagementresponse>> UpdateApplicantDetails(updateApplicantRequest request)
        {
            
            var user = await _context.ApplicationForms.AsNoTracking()
                .Where(x => x.ApplicantId == request.applicantId)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                if (string.Equals(request.VerificationStatusEid, "APPROVED", StringComparison.OrdinalIgnoreCase))
                {
                    user.VerificationStatusEid = (int)VerificationStatus.APPROVED;
                }
                else
                {                
                    user.VerificationStatusEid = (int)VerificationStatus.REJECTED;
                }
                _context.ApplicationForms.Update(user);
                await _context.SaveChangesAsync();

                return user.Adapt<List<Applicantmanagementresponse>>();
            }
            return new List<Applicantmanagementresponse>();
        }

    }

}
