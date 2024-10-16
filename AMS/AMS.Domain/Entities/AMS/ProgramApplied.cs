using AMS.DOMAIN.Entities.Lookups;


namespace AMS.DOMAIN.Entities.AMS
{
    public class ProgramApplied
    {
        public int Id { get; set; }
        public int ApplicationFormId { get; set; }
        public int DepartmentId { get; set; }
        public int TimeShiftId { get; set; }
        public int PreferenceNo { get; set; }
        public virtual ApplicationForm? ApplicationForm { get; set; }
        public virtual Department? Department { get; set; }
        public virtual TimeShift? TimeShift { get; set; }
        public int? InsertedBy { get; set; }
        public DateTime? InsertedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
