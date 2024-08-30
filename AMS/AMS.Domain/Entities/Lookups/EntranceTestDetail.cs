using AMS.DOMAIN.Base;
using AMS.DOMAIN.Entities.AMS;

namespace AMS.DOMAIN.Entities.Lookups
{
    public class EntranceTestDetail : IBaseEntity
    {

        public int Id { get; set; }
        public DateTime TestDate { get; set; }
        public DateTime TestValideTill { get; set; }
        public  int TestMarks { get; set; }
        public int TestTypeId { get; set; }
        public int ApplicantId { get; set; }
        public virtual TestType? TestType { get; set; }
        public virtual Applicant? Applicant { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
