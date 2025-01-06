using AMS.MODELS.ApplicantManagement;
using AMS.MODELS.ApplicationForm.Applicant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SERVICES.IDataService
{
    public interface IApplicantManagementService
    {
        Task<List<ApplicantInfoList>> GetAllApplicantDetails(ApplicantInfoRequest user);
        Task<List<Applicantmanagementresponse>> UpdateApplicantDetails(updateApplicantRequest request);
    }
}
