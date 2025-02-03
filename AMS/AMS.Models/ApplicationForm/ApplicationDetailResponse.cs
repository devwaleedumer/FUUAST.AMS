using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.ApplicationForm
{
    public class ApplicationDetailResponse
    {
        public string FullName { get; set; } = default!;
        public int  FormNo { get; set; }
        public string Cnic { get; set; } = default!;
        public int FeeChallanNo { get; set; } = default!;
        public string Program { get; set; } = default!;
        public int NoOfProgramsApplied { get; set; }
        public double TotalFee { get; set; }
        public string ChallanStatus { get; set; } = default!;
        public string VerificationStatus { get; set; } = default!;
    }
}
