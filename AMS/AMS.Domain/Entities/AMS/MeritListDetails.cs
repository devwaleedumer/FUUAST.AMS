using AMS.DOMAIN.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.AMS
{
    public class MeritListDetails : IBaseEntity
    {
        public int Id { get; set; }
        public int MeritListId { get; set; }
        public int ApplicationFormId { get; set; }
        public double? Score { get; set; }
        public int? Status { get; set; }
        public virtual MeritList? MeritList { get; set; }
        public virtual ApplicationForm? ApplicationForm { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
