using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.Dashboard
{
    public class ApplicantDashboardResponse
    {
        public ApplicantDashboardResponse()
        {
            FormStatuses = new();
        }
        public List<FormStatus> FormStatuses { get; set; }
        public DateTime? LastModified { get; set; }
        public int CompletedSteps { get; set; }
    }

    public class DashboardResponse()
    {
        public int? TotalApplicant {  get; set; }
        public int? TotalUser {  get; set; }
        public int? ApprovedApplication { get; set; }
        public int ?SubmittedApplication {  get; set; }
    }
}
