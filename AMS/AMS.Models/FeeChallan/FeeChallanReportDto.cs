using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.MODELS.FeeChallan
{
    public class FeeChallanReportDto
    {
        public string ApplicationFormNo { get; set; } = default!;
        public int VoucherNo { get; set; }
        public string CNIC { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string FatherName { get; set; } = default!;
        public string AdmissionSession { get; set; } = default!;
        public string Program { get; set; } = default!;
        public int NoOfProgramsApplied { get; set; }
        public string AmountInWords { get; set; } = default!;
    }
}
