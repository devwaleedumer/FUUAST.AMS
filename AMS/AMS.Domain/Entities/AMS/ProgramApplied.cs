using AMS.DOMAIN.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.AMS
{
    public class ProgramApplied
    {
        public int Id { get; set; }
        public int ApplicationFormId { get; set; }
        public int ProgramId { get; set; }
        public int PriorityEid { get; set; }

        public virtual ApplicationForm? ApplicationForm { get; set; }
        public virtual Program? Program { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
