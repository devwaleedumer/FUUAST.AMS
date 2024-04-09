using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.AMS
{
    public class ApplicationForm : IBaseEntity
    {
        public ApplicationForm()
        {
            ProgramsApplied=new HashSet<ProgramApplied>();
        }
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int? ApplicantId { get; set; }
        public bool? InfoConsent { get; set; }
        public int? StatusEid { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public bool IsSubmitted { get; set; }
        public virtual Applicant? Applicant { get; set; }
        public virtual FeeChallan? FeeChallan { get; set; }
        public virtual ICollection<ProgramApplied> ProgramsApplied { get; set; }
        public virtual AdmissionSession? Session { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
