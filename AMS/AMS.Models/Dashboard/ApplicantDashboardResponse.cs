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
}
