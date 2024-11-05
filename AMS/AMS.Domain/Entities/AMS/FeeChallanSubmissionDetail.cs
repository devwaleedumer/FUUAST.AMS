using AMS.DOMAIN.Base;

namespace AMS.DOMAIN.Entities.AMS
{
    public class FeeChallanSubmissionDetail: IBaseEntity
    {
        public int Id { get; set; }
        public  int BranchCode { get; set; }
        public required string BranchNameWithCity { get; set; }
        public  DateTime SubmissionDate { get; set; }
        public required string DocumentUrl { get; set; }
        public int FeeChallanId { get; set; }
        public virtual FeeChallan? FeeChallan { get; set; }

        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
