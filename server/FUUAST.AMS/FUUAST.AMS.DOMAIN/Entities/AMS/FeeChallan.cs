using FUUAST.AMS.DOMAIN.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Entities.AMS
{
    public class FeeChallan : IBaseEntity
    {
        public int Id { get; set; }
        public int NoProgramsApplied { get; set; }
        public int TotalFee { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime DueTill { get; set; }
        public int ApplicationFormId { get; set; }
        public virtual FeeChallanSubmissionDetail FeeChallanSubmissionDetail { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
