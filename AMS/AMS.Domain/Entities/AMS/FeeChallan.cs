using AMS.DOMAIN.Base;


namespace AMS.DOMAIN.Entities.AMS
{
    public class FeeChallan : IBaseEntity
    {
        public int Id { get; set; }
        public int NoOfProgramsApplied { get; set; }
        public int TotalFee { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime DueTill { get; set; }
        public int ApplicationFormId { get; set; }
        public virtual FeeChallanSubmissionDetail? FeeChallanSubmissionDetail { get; set; }
        public virtual ApplicationForm? ApplicationForm { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
