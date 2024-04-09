using AMS.DOMAIN.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DOMAIN.Entities.AMS
{
    public class Guardian : IBaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Occupation { get; set; }
        public required string  Relation { get; set; }
        public decimal TotalPerMonthIncome { get; set; }
        public decimal TotalPerMonthExpenses { get; set; }
        public required string PhoneNo { get; set; }
        public int ApplicantId { get; set; }
        public virtual Applicant? Applicant { get; set; }


        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
